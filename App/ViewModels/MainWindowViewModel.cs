using System;
using System.Threading;
using System.Threading.Tasks;
using App.Models;
using App.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Org.OpenAPITools.Api;

namespace App.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly IUserApi _userApi = null!;
    private readonly AppState _state = null!;

    public LoginViewModel LoginVm { get; } = null!;
    public DashboardViewModel DashboardVm { get; } = null!;
    public AdminViewModel AdminVm { get; } = null!;
    public ChangePasswordViewModel ChangePwVm { get; } = null!;

    [ObservableProperty]
    private int _selectedTabIndex;

    [ObservableProperty]
    private bool _showChangePw;

    public bool IsLoggedIn => _state.IsLoggedIn;
    public bool IsAdmin => _state.IsAdmin;

    public MainWindowViewModel() { }

    public MainWindowViewModel(IUserApi userApi, IAdminApi adminApi, AppState state,
        LoginViewModel loginVm, DashboardViewModel dashboardVm, AdminViewModel adminVm,
        ChangePasswordViewModel changePwVm)
    {
        _userApi = userApi;
        _state = state;
        LoginVm = loginVm;
        DashboardVm = dashboardVm;
        AdminVm = adminVm;
        ChangePwVm = changePwVm;

        _state.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(AppState.IsLoggedIn) ||
                e.PropertyName == nameof(AppState.IsAdmin))
            {
                OnPropertyChanged(nameof(IsLoggedIn));
                OnPropertyChanged(nameof(IsAdmin));
                if (_state.IsLoggedIn)
                {
                    ShowChangePw = false;
                    SelectedTabIndex = 1;
                }
                else
                {
                    SelectedTabIndex = 0;
                }
            }
            else if (e.PropertyName == nameof(AppState.MustChangePw))
            {
                if (_state.MustChangePw)
                {
                    ShowChangePw = true;
                    SelectedTabIndex = 3;
                }
                else
                {
                    ShowChangePw = false;
                    SelectedTabIndex = 0;
                }
            }
        };
    }

    [RelayCommand]
    private async Task Logout()
    {
        try
        {
            await _userApi.LogoutApiAsync(CancellationToken.None);
        }
        catch { }

        AuthPersistenceService.Clear();
        _state.Clear();
        ShowChangePw = false;
        SelectedTabIndex = 0;
    }
}