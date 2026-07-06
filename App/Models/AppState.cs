using CommunityToolkit.Mvvm.ComponentModel;
using Org.OpenAPITools.Model;

namespace App.Models;

public partial class AppState : ObservableObject
{
    [ObservableProperty]
    private bool _isLoggedIn;

    [ObservableProperty]
    private bool _isAdmin;

    [ObservableProperty]
    private long _userId;

    [ObservableProperty]
    private string _username = "";

    [ObservableProperty]
    private string? _token;

    [ObservableProperty]
    private string _serverUrl = "";

    [ObservableProperty]
    private bool _mustChangePw;

    [ObservableProperty]
    private WorkerSummary? _whoami;

    public void Clear()
    {
        IsLoggedIn = false;
        IsAdmin = false;
        UserId = 0;
        Username = "";
        Token = null;
        Whoami = null;
    }

    public void SetLogin(LoginOutput output)
    {
        Token = output.Token;
        UserId = output.Id;
        Username = output.Username;
    }
}