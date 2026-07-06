using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using App.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Client;
using Org.OpenAPITools.Model;

namespace App.ViewModels;

public partial class AdminViewModel : ViewModelBase
{
    private readonly IAdminApi _adminApi = null!;
    private readonly AppState _state = null!;

    public bool IsEditingSelf => _state.Whoami?.Id.ToString() == EwId;
    public bool IsNotEditingSelf => !IsEditingSelf;

    public AdminViewModel() { }

    public AdminViewModel(IAdminApi adminApi, AppState state)
    {
        _adminApi = adminApi;
        _state = state;
        if (state.IsLoggedIn) _ = LoadWorkers();
        state.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(AppState.IsLoggedIn) && state.IsLoggedIn)
                _ = LoadWorkers();
        };
    }

    #region Workers

    [ObservableProperty] private WorkerSummary? _selectedWorker;
    [ObservableProperty] private bool _hasSelectedWorker;
    [ObservableProperty] private string _wStatus = "";
    public ObservableCollection<WorkerSummary> Workers { get; } = new();

    partial void OnSelectedWorkerChanged(WorkerSummary? value)
    {
        HasSelectedWorker = value is not null;
        if (value is null) return;
        EwId = value.Id.ToString();
        EwName = value.Name;
        EwAddress = value.Address;
        EwPhone = value.Phone;
        EwEmail = value.Email;
        EwHourly = FmtCents(value.RateHourlyCents);
        EwMileage = FmtCents(value.RateMileageCents);
        EwDrivetime = FmtCents(value.RateDriveHourlyCents);
        EwFlatrate = FmtCents(value.FlatRateCents);
        EwAdmin = value.Admin;
        EwStatus = "";
        OnPropertyChanged(nameof(IsEditingSelf));
        OnPropertyChanged(nameof(IsNotEditingSelf));
    }

    [RelayCommand]
    private async Task LoadWorkers()
    {
        var selectedId = SelectedWorker?.Id;
        try
        {
            var list = (await _adminApi.ListUsersApiAsync(CancellationToken.None)).Ok();
            Workers.Clear();
            if (list is not null) foreach (var w in list) Workers.Add(w);
            SelectedWorker = Workers.FirstOrDefault(w => w.Id == selectedId);
            WStatus = $"{Workers.Count} workers loaded.";
        }
        catch (Exception ex) { WStatus = $"Error: {ex.Message}"; }
    }

    // ── Edit Worker ──

    [ObservableProperty] private string _ewId = "";
    [ObservableProperty] private string _ewName = "";
    [ObservableProperty] private string _ewAddress = "";
    [ObservableProperty] private string _ewPhone = "";
    [ObservableProperty] private string _ewEmail = "";
    [ObservableProperty] private string _ewHourly = "";
    [ObservableProperty] private string _ewMileage = "";
    [ObservableProperty] private string _ewDrivetime = "";
    [ObservableProperty] private string _ewFlatrate = "";
    [ObservableProperty] private bool _ewAdmin;
    [ObservableProperty] private string _ewStatus = "";

    [RelayCommand]
    private async Task SaveWorker()
    {
        if (!long.TryParse(EwId, out var id) || string.IsNullOrWhiteSpace(EwName))
        { EwStatus = "ID and Name required."; return; }
        try
        {
            var input = new WorkerChangeInput(EwAddress, EwDrivetime, EwEmail, EwFlatrate,
                EwHourly, id, EwMileage, EwName, EwPhone, admin: EwAdmin ? true : null);
            var result = (await _adminApi.ChangeWorkerApiAsync(input, CancellationToken.None)).Ok();
            EwStatus = result is not null ? $"Worker #{result.Id} saved." : "Failed.";
            if (result is not null) await LoadWorkers();
        }
        catch (Exception ex) { EwStatus = $"Error: {ex.Message}"; }
    }

    // ── Actions on selected worker ──

    [RelayCommand]
    private async Task DeactivateWorker()
    {
        if (SelectedWorker is null) return;
        try { await _adminApi.DeactivateApiAsync(new DeactivateInput(SelectedWorker.Id), CancellationToken.None); await LoadWorkers(); }
        catch (Exception ex) { WStatus = $"Error: {ex.Message}"; }
    }

    [RelayCommand]
    private async Task RestoreWorker()
    {
        if (SelectedWorker is null) return;
        try { await _adminApi.RestoreApiAsync(new RestoreInput(SelectedWorker.Id), CancellationToken.None); await LoadWorkers(); }
        catch (Exception ex) { WStatus = $"Error: {ex.Message}"; }
    }

    [RelayCommand]
    private async Task ResetWorkerPassword()
    {
        if (SelectedWorker is null) return;
        try { await _adminApi.ResetPwApiAsync(new ResetPwInput(SelectedWorker.Id), CancellationToken.None); WStatus = $"Password reset for worker #{SelectedWorker.Id}."; }
        catch (Exception ex) { WStatus = $"Error: {ex.Message}"; }
    }

    [RelayCommand]
    private async Task ForceLogoutWorker()
    {
        if (SelectedWorker is null) return;
        try { await _adminApi.LogoutUserApiAsync(new LogoutForm(SelectedWorker.Id), CancellationToken.None); WStatus = $"Worker #{SelectedWorker.Id} logged out."; }
        catch (Exception ex) { WStatus = $"Error: {ex.Message}"; }
    }

    // ── Create Worker (separate form) ──

    [ObservableProperty] private string _cwName = "";
    [ObservableProperty] private string _cwAddress = "";
    [ObservableProperty] private string _cwPhone = "";
    [ObservableProperty] private string _cwEmail = "";
    [ObservableProperty] private string _cwHourly = "0";
    [ObservableProperty] private string _cwMileage = "0";
    [ObservableProperty] private string _cwDrivetime = "0";
    [ObservableProperty] private string _cwFlatrate = "0";
    [ObservableProperty] private bool _cwAdmin;
    [ObservableProperty] private string _cwStatus = "";

    [RelayCommand]
    private async Task CreateWorker()
    {
        if (string.IsNullOrWhiteSpace(CwName)) { CwStatus = "Name required."; return; }
        try
        {
            var input = new WorkerCreateInput(CwAddress, CwDrivetime, CwEmail, CwFlatrate,
                CwHourly, CwMileage, CwName, CwPhone, admin: CwAdmin ? true : null);
            var result = (await _adminApi.CreateWorkerApiAsync(input, CancellationToken.None)).Created();
            CwStatus = result is not null ? $"Created worker #{result.Id}." : "Failed.";
            if (result is not null) { CwName = CwAddress = CwPhone = CwEmail = CwHourly = CwMileage = CwDrivetime = CwFlatrate = ""; CwAdmin = false; await LoadWorkers(); }
        }
        catch (Exception ex) { CwStatus = $"Error: {ex.Message}"; }
    }

    #endregion

    #region Export

    [ObservableProperty] private string _exportStatus = "";

    public async Task<byte[]?> ExportDatabaseAsync()
    {
        try
        {
            var r = await _adminApi.ExportDbApiAsync(CancellationToken.None);
            if (r.IsSuccessStatusCode)
            {
                var bytes = System.Text.Encoding.Latin1.GetBytes(r.RawContent);
                ExportStatus = bytes.Length > 0 ? "Export ready." : "Export returned empty.";
                return bytes.Length > 0 ? bytes : null;
            }
            ExportStatus = $"Export failed (status {r.StatusCode}).";
        }
        catch (Exception ex) { ExportStatus = $"Error: {ex.Message}"; }
        return null;
    }

    #endregion

    #region Worker Data

    [ObservableProperty] private DateTime? _wdStartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
    [ObservableProperty] private DateTime? _wdEndDate = DateTime.Today;
    [ObservableProperty] private string _wdStatus = "";
    [ObservableProperty] private string _wdFrom = "";
    [ObservableProperty] private string _wdTo = "";
    [ObservableProperty] private int _wdNumJobs;

    public ObservableCollection<WdRow> WdEntries { get; } = new();
    [ObservableProperty] private WdRow? _wdTotals;

    [RelayCommand]
    private async Task FetchWorkerData()
    {
        try
        {
            var worker = SelectedWorker is not null
                ? new Option<long?>(SelectedWorker.Id)
                : default(Option<long?>);
            var startDate = WdStartDate.HasValue
                ? new Option<DateOnly?>(DateOnly.FromDateTime(WdStartDate.Value))
                : default(Option<DateOnly?>);
            var endDate = WdEndDate.HasValue
                ? new Option<DateOnly?>(DateOnly.FromDateTime(WdEndDate.Value))
                : default(Option<DateOnly?>);

            var result = (await _adminApi.WorkerdatapageApiAsync(
                worker, startDate, endDate, CancellationToken.None)).Ok();

            WdEntries.Clear();
            if (result is not null)
            {
                int i = 0;
                foreach (var e in result.Entries)
                    WdEntries.Add(new WdRow(e, i++));

                WdFrom = result.From;
                WdTo = result.To;
                WdNumJobs = result.NumJobs;
                WdTotals = new WdRow(result.Totals, -1, isTotal: true);
                WdStatus = $"{result.Entries.Count} entries ({WdFrom} to {WdTo}), {WdNumJobs} jobs.";
            }
            else
            {
                WdFrom = WdTo = "";
                WdNumJobs = 0;
                WdTotals = null;
                WdStatus = "No data.";
            }
        }
        catch (Exception ex) { WdStatus = $"Error: {ex.Message}"; }
    }

    #endregion

    private static string FmtCents(long c) => (c / 100.0).ToString("F2");
}