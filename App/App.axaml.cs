using System;
using System.Net.Http;
using System.Threading;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Client;
using Org.OpenAPITools.Extensions;
using App.Models;
using App.Services;
using App.ViewModels;
using App.Views;

namespace App;

public partial class App : Application
{
    public static IServiceProvider Services { get; private set; } = null!;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var services = new ServiceCollection();

        services.AddLogging();
        services.AddSingleton<AppState>();
        services.AddTransient<ServerUrlHandler>();

        services.AddApi(options =>
        {
            options.UseProvider<AuthenticationTokenProvider, BearerToken>();
            options.AddApiHttpClients(
                client: (sp, client) =>
                {
                    var url = sp.GetRequiredService<AppState>().ServerUrl;
                    client.BaseAddress = new Uri(string.IsNullOrEmpty(url) ? "http://localhost" : url);
                },
                builder: b => b.AddHttpMessageHandler(sp => sp.GetRequiredService<ServerUrlHandler>()));
        });

        // ViewModels
        services.AddSingleton<LoginViewModel>();
        services.AddSingleton<DashboardViewModel>();
        services.AddSingleton<AdminViewModel>();
        services.AddSingleton<ChangePasswordViewModel>();
        services.AddSingleton<MainWindowViewModel>();

        Services = services.BuildServiceProvider();

        TryAutoLogin(Services);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = Services.GetRequiredService<MainWindowViewModel>(),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static async void TryAutoLogin(IServiceProvider sp)
    {
        var saved = AuthPersistenceService.Load();
        if (string.IsNullOrEmpty(saved.ServerUrl) || string.IsNullOrEmpty(saved.Token))
            return;

        var state = sp.GetRequiredService<AppState>();
        state.ServerUrl = saved.ServerUrl;
        state.Token = saved.Token;

        try
        {
            var userApi = sp.GetRequiredService<IUserApi>();
            var refresh = await userApi.RefreshTokenApiAsync(CancellationToken.None);
            var login = refresh.Ok();
            if (login?.Token is null) { AuthPersistenceService.Clear(); state.Clear(); return; }

            state.SetLogin(login);

            var whoami = (await userApi.WhoamiAsync(CancellationToken.None)).Ok();
            if (whoami is null) { AuthPersistenceService.Clear(); state.Clear(); return; }

            state.Whoami = whoami;
            state.IsAdmin = whoami.Admin;
            state.IsLoggedIn = true;
            AuthPersistenceService.Save(state.ServerUrl, state.Token!);
        }
        catch { AuthPersistenceService.Clear(); state.Clear(); }
    }
}
