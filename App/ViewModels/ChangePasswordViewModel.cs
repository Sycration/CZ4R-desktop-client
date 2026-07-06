using System;
using System.Threading;
using System.Threading.Tasks;
using App.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Model;

namespace App.ViewModels;

public partial class ChangePasswordViewModel : ViewModelBase
{
    private readonly IUserApi _userApi = null!;
    private readonly AppState _state = null!;

    [ObservableProperty]
    private string _password1 = "";

    [ObservableProperty]
    private string _password2 = "";

    [ObservableProperty]
    private string _status = "";

    public ChangePasswordViewModel() { }

    public ChangePasswordViewModel(IUserApi userApi, AppState state)
    {
        _userApi = userApi;
        _state = state;
    }

    [RelayCommand]
    private async Task ChangePassword()
    {
        if (Password1 != Password2) { Status = "Passwords don't match."; return; }
        if (string.IsNullOrWhiteSpace(Password1)) { Status = "Password required."; return; }

        try
        {
            var input = new ChangePwInput(_state.UserId, Password1, Password2);
            var result = (await _userApi.ChangePwApiAsync(input, CancellationToken.None)).Ok();
            if (result is not null && result.Changed)
            {
                Status = "Password changed. Please log in with your new password.";
                _state.Clear();
                _state.MustChangePw = false;
            }
            else
            {
                Status = "Password change failed.";
            }
        }
        catch (Exception ex) { Status = $"Error: {ex.Message}"; }
    }
}