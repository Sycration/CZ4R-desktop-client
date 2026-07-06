using System;
using System.Threading;
using System.Threading.Tasks;
using App.Models;
using App.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Model;

namespace App.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    private readonly IUserApi _userApi = null!;
    private readonly AppState _state = null!;

    [ObservableProperty]
    private string _username = "";

    [ObservableProperty]
    private string _password = "";

    [ObservableProperty]
    private string _serverUrl = "";

    [ObservableProperty]
    private string _statusMessage = "";

    [ObservableProperty]
    private bool _isLoggingIn;

    public LoginViewModel() { }

    public LoginViewModel(IUserApi userApi, AppState state)
    {
        _userApi = userApi;
        _state = state;
        _serverUrl = state.ServerUrl;
    }

    [RelayCommand]
    private async Task Login()
    {
        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
        {
            StatusMessage = "Username and password are required.";
            return;
        }

        IsLoggingIn = true;
        StatusMessage = "Logging in...";
        _state.ServerUrl = ServerUrl.TrimEnd('/');

        try
        {
            var response = await _userApi.LoginApiAsync(new LoginForm(Password, Username), CancellationToken.None);
            var result = response.Ok();

            if (result is null)
            {
                StatusMessage = "Login failed: no response.";
                IsLoggingIn = false;
                return;
            }

            _state.SetLogin(result);

            if (result.MustChangePw)
            {
                _state.MustChangePw = true;
                StatusMessage = "";
                Password = "";
                IsLoggingIn = false;
                return;
            }

            AuthPersistenceService.Save(_state.ServerUrl, result.Token!);

            var whoamiResponse = await _userApi.WhoamiAsync(CancellationToken.None);
            var whoami = whoamiResponse.Ok();

            if (whoami is not null)
            {
                _state.Whoami = whoami;
                _state.IsAdmin = whoami.Admin;
                _state.IsLoggedIn = true;
                StatusMessage = $"Logged in as {whoami.Name}" +
                                (whoami.Admin ? " (admin)." : ".");
                Password = "";
            }
            else
            {
                _state.Clear();
                StatusMessage = "Failed to fetch user details after login.";
            }
        }
        catch (Exception ex)
        {
            _state.Clear();
            StatusMessage = $"Login failed: {ex.Message}";
        }
        finally
        {
            IsLoggingIn = false;
        }
    }
}