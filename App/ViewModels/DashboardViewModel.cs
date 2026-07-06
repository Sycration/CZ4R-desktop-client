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

public enum RightPanelMode { None, Detail, Editor }

public partial class DashboardViewModel : ViewModelBase
{
    private readonly IUserApi _userApi = null!;
    private readonly IAdminApi _adminApi = null!;
    private readonly AppState _state = null!;

    public bool IsAdmin => _state.IsAdmin;
    public bool DetailVisible => RightPanel == RightPanelMode.Detail;
    public bool EditorVisible => RightPanel == RightPanelMode.Editor;

    public DashboardViewModel() { }

    public DashboardViewModel(IUserApi userApi, IAdminApi adminApi, AppState state)
    {
        _userApi = userApi;
        _adminApi = adminApi;
        _state = state;
        if (state.IsLoggedIn) { _ = FetchStats(); _ = FetchJobs(); }
        state.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(AppState.IsLoggedIn) && state.IsLoggedIn)
            { _ = FetchStats(); _ = FetchJobs(); _ = LoadFilterWorkers(); }
            else if (e.PropertyName == nameof(AppState.IsAdmin))
                OnPropertyChanged(nameof(IsAdmin));
        };
    }

    // ══════ STATS ═══════════════════════════════════════════

    [ObservableProperty] private string _jobsCount = "-";
    [ObservableProperty] private string _workersCount = "-";
    [ObservableProperty] private string _milesCount = "-";
    [ObservableProperty] private string _statsDetail = "";

    [RelayCommand]
    private async Task FetchStats()
    {
        try
        {
            var s = (await _userApi.IndexApiAsync(CancellationToken.None)).Ok();
            if (s is not null)
            {
                JobsCount = $"{s.Jobs}"; WorkersCount = $"{s.Workers}"; MilesCount = s.Miles;
                StatsDetail = $"Avgs: {s.JobsAvg} jobs | {s.WorkersAvg} workers | {s.MilesAvg} miles";
            }
        }
        catch { StatsDetail = "Stats unavailable"; }
    }

    // ══════ JOB LIST ════════════════════════════════════════

    [ObservableProperty] private DateTime? _jlStartDate = DateTime.Today;
    [ObservableProperty] private DateTime? _jlEndDate = DateTime.Today.AddDays(15);
    [ObservableProperty] private string _jlSiteName = "";
    [ObservableProperty] private string _jlWorkOrder = "";
    [ObservableProperty] private string _jlAddress = "";
    [ObservableProperty] private string _jlNotes = "";
    [ObservableProperty] private bool _jlAssigned = true;
    [ObservableProperty] private bool _jlStarted = true;
    [ObservableProperty] private bool _jlCompleted = true;
    [ObservableProperty] private string _jlOrder = "Latest";
    [ObservableProperty] private string _jlWorkers = "";

    [ObservableProperty] private string _jlStatus = "";

    public ObservableCollection<WorkerFilterItem> FilterWorkers { get; } = new();

    [RelayCommand] private void SelectAllWorkers() { foreach (var w in FilterWorkers) w.IsSelected = true; }
    [RelayCommand] private void DeselectAllWorkers() { foreach (var w in FilterWorkers) w.IsSelected = false; }

    private async Task LoadFilterWorkers()
    {
        if (!_state.IsAdmin) return;
        try
        {
            var list = (await _adminApi.ListUsersApiAsync(CancellationToken.None)).Ok();
            if (list is null) return;
            FilterWorkers.Clear();
            foreach (var w in list.Where(w => !w.Deactivated))
            {
                var item = new WorkerFilterItem(w.Id, w.Name, true);
                item.PropertyChanged += (_, _) => JlWorkers = BuildWorkersFilter();
                FilterWorkers.Add(item);
            }
            JlWorkers = BuildWorkersFilter();
        }
        catch { }
    }

    private string BuildWorkersFilter()
    {
        var ids = FilterWorkers.Where(w => w.IsSelected).Select(w => w.Id.ToString()).ToArray();
        return ids.Length == 0 || ids.Length == FilterWorkers.Count ? "" : string.Join("-", ids);
    }

    public ObservableCollection<JobRow> Jobs { get; } = new();

    [RelayCommand]
    private async Task FetchJobs()
    {
        try
        {
            var r = await _userApi.JoblistApiAsync(
                startDate: JlStartDate.HasValue ? DateOnly.FromDateTime(JlStartDate.Value) : default(Option<DateOnly?>),
                endDate: JlEndDate.HasValue ? DateOnly.FromDateTime(JlEndDate.Value) : default(Option<DateOnly?>),
                siteName: Opt(JlSiteName), workOrder: Opt(JlWorkOrder), address: Opt(JlAddress),
                notes: Opt(JlNotes), order: JlOrder == "Earliest" ? Order.Earliest : Order.Latest,
                assigned: JlAssigned ? true : default(Option<bool?>),
                started: JlStarted ? true : default(Option<bool?>),
                completed: JlCompleted ? true : default(Option<bool?>),
                workers: Opt(JlWorkers), cancellationToken: CancellationToken.None);
            var result = r.Ok();
            if (result is not null) { Jobs.Clear(); int i = 0; foreach (var j in result.Jobs) Jobs.Add(new JobRow(j, i++)); JlStatus = $"{result.Count} jobs"; }
        }
        catch (Exception ex) { JlStatus = $"Error: {ex.Message}"; }
    }

    // ══════ RIGHT PANEL STATE ═══════════════════════════════

    [ObservableProperty]
    private RightPanelMode _rightPanel;

    partial void OnRightPanelChanged(RightPanelMode value)
    {
        OnPropertyChanged(nameof(DetailVisible));
        OnPropertyChanged(nameof(EditorVisible));
    }

    public void SwitchToDetail()
    {
        RightPanel = RightPanelMode.Detail;
        if (SelectedJob is not null)
        {
            PopulateCheckInOut(SelectedJob);
            _ = FetchCurrentAssignmentData(SelectedJob);
        }
    }

    private async Task FetchCurrentAssignmentData(JobData job)
    {
        var wid = job.WorkerId ?? _state.UserId;
        if (wid == 0) return;
        try
        {
            var d = (await _userApi.AssignmentDataApiAsync(job.JobId, wid, CancellationToken.None)).Ok();
            if (d is null) return;
            CioSignin = ParseTimeSpan(d.Signin);
            CioSignout = ParseTimeSpan(d.Signout);
            CioMilesDriven = d.MilesDriven.ToString("F2");
            CioHoursDriven = d.HoursDriven.ToString("F2");
            CioMinutesDriven = d.MinutesDriven.ToString("F2");
            CioExtraExpenses = (d.Extraexpcents / 100.0).ToString("F2");
            CioNotes = d.WorkerNotes ?? "";
        }
        catch { }
    }

    private static TimeSpan? ParseTimeSpan(string? s)
    {
        if (string.IsNullOrWhiteSpace(s)) return null;
        if (TimeSpan.TryParse(s, out var t)) return t;
        if (DateTime.TryParse(s, out var dt)) return dt.TimeOfDay;
        return null;
    }

    [ObservableProperty] private JobData? _selectedJob;

    partial void OnSelectedJobChanged(JobData? value)
    {
    }

    private void PopulateCheckInOut(JobData job)
    {
        CioJobId = job.JobId.ToString();
        CioWorkerId = job.WorkerId?.ToString() ?? "";
        CioSignin = CioSignout = null;
        CioNotes = "";
        CioMilesDriven = CioHoursDriven = CioMinutesDriven = "0";
        CioExtraExpenses = "0.00";
        CioStatus = ""; AdResult = "";
    }

    // ══════ CHECK IN / OUT ══════════════════════════════════

    [ObservableProperty] private string _cioJobId = "";
    [ObservableProperty] private string _cioWorkerId = "";
    [ObservableProperty] private TimeSpan? _cioSignin;
    [ObservableProperty] private TimeSpan? _cioSignout;
    [ObservableProperty] private string _cioMilesDriven = "";
    [ObservableProperty] private string _cioHoursDriven = "";
    [ObservableProperty] private string _cioMinutesDriven = "";
    [ObservableProperty] private string _cioExtraExpenses = "";
    [ObservableProperty] private string _cioNotes = "";
    [ObservableProperty] private string _cioStatus = "";

    [RelayCommand]
    private async Task SubmitCheckInOut()
    {
        if (!long.TryParse(CioJobId, out var jid) || !long.TryParse(CioWorkerId, out var wid))
        { CioStatus = "Valid Job ID and Worker ID required."; return; }
        try
        {
            var input = new CheckInOutInput(jid, wid)
            {
                Signin = CioSignin.HasValue ? CioSignin.Value.ToString(@"hh\:mm") : null,
                Signout = CioSignout.HasValue ? CioSignout.Value.ToString(@"hh\:mm") : null,
                MilesDriven = ParseFloat(CioMilesDriven), HoursDriven = ParseFloat(CioHoursDriven),
                MinutesDriven = ParseFloat(CioMinutesDriven), ExtraExpensesCents = ParseCents(CioExtraExpenses),
                Notes = NullIfEmpty(CioNotes),
            };
            var result = (await _userApi.CheckinoutApiAsync(input, CancellationToken.None)).Ok();
            CioStatus = result is not null ? $"Updated job #{result.JobId}, worker #{result.WorkerId}." : "No response.";
            await FetchJobs();
            HighlightSingle(jid, wid);
        }
        catch (Exception ex) { CioStatus = $"Error: {ex.Message}"; }
    }

    [ObservableProperty] private string _adResult = "";
    [ObservableProperty] private bool _hasAdResult;
    partial void OnAdResultChanged(string value) => HasAdResult = !string.IsNullOrEmpty(value);

    [RelayCommand]
    private async Task FetchAssignmentData()
    {
        if (SelectedJob is null) return;
        var wid = long.TryParse(CioWorkerId, out var id) ? id : _state.UserId;
        try
        {
            var d = (await _userApi.AssignmentDataApiAsync(SelectedJob.JobId, wid, CancellationToken.None)).Ok();
            AdResult = d is null ? "" : FormatAssignmentData(d);
        }
        catch (Exception ex) { AdResult = $"Error: {ex.Message}"; }
    }

    // ══════ JOB EDITOR ══════════════════════════════════════

    [ObservableProperty] private bool _jobEditorHasId;
    [ObservableProperty] private string _jobEditorTitle = "Create Job";
    [ObservableProperty] private string _jeAddress = "";
    public ObservableCollection<AssignmentRow> AssignmentRows { get; } = new();
    [ObservableProperty] private DateTime? _jeDate;
    [ObservableProperty] private string _jeNotes = "";
    [ObservableProperty] private string _jeServcode = "";
    [ObservableProperty] private string _jeSitename = "";
    [ObservableProperty] private string _jeWorkorder = "";
    [ObservableProperty] private string _jeStatus = "";
    private long? _editingJobId;
    public long? EditingJobId => _editingJobId;

    [RelayCommand]
    private void BeginCreateJob()
    {
        ClearHighlighting();
        _editingJobId = null;
        JobEditorHasId = false;
        JobEditorTitle = "Create Job";
        JeAddress = JeNotes = JeServcode = JeSitename = JeWorkorder = "";
        JeDate = null;
        AssignmentRows.Clear();
        JeStatus = "";
        RightPanel = RightPanelMode.Editor;
        _ = LoadWorkersForAssignment();
    }

    public void BeginEditJob(JobData data)
    {
        ClearHighlighting();
        HighlightByJobId(data.JobId);
        RightPanel = RightPanelMode.Editor;
        _ = LoadJobForEdit(data.JobId);
    }

    private void ClearHighlighting()
    {
        foreach (var row in Jobs) row.IsHighlighted = false;
    }

    private void HighlightByJobId(long jobId)
    {
        foreach (var row in Jobs)
            if (row.Data.JobId == jobId) row.IsHighlighted = true;
    }

    public void HighlightSingle(long jobId, long? workerId)
    {
        ClearHighlighting();
        foreach (var row in Jobs)
            if (row.Data.JobId == jobId && row.Data.WorkerId == workerId)
            { row.IsHighlighted = true; return; }
    }

    private async Task LoadWorkersForAssignment()
    {
        if (!_state.IsAdmin) return;
        try
        {
            var list = (await _adminApi.ListUsersApiAsync(CancellationToken.None)).Ok();
            if (list is null) return;
            AssignmentRows.Clear();
            foreach (var w in list.Where(w => !w.Deactivated))
                AssignmentRows.Add(new AssignmentRow(w.Id, w.Name, false, false));
        }
        catch { }
    }

    private async Task LoadJobForEdit(long jobId)
    {
        try
        {
            var page = (await _adminApi.JobeditpageApiAsync(jobId, CancellationToken.None)).Ok();
            if (page is null) { JeStatus = "Job not found."; return; }
            _editingJobId = jobId;
            JobEditorHasId = true;
            JobEditorTitle = "Edit Job";
            if (page.Job is not null)
            {
                JeSitename = page.Job.Sitename; JeServcode = page.Job.Servicecode;
                JeWorkorder = page.Job.Workorder; JeAddress = page.Job.Address;
                JeDate = DateTime.TryParse(page.Job.Date, out var d) ? d : null;
                JeNotes = page.Job.Notes;
            }
            AssignmentRows.Clear();
            foreach (var w in page.Workers)
                AssignmentRows.Add(new AssignmentRow(w.Id, w.Name, w.Assigned, w.FlatRate));
            JeStatus = "";
        }
        catch (Exception ex) { JeStatus = $"Error loading job: {ex.Message}"; }
    }

    [RelayCommand]
    private void CancelEdit()
    {
        ClearHighlighting();
        _editingJobId = null;
        RightPanel = SelectedJob is not null ? RightPanelMode.Detail : RightPanelMode.None;
    }

    [RelayCommand]
    private async Task SaveJob()
    {
        if (JeDate is not { } dt) { JeStatus = "Select a date."; return; }
        var date = DateOnly.FromDateTime(dt);
        var assignments = AssignmentRows
            .Where(a => a.IsAssigned)
            .Select(a => new JobAssignment(a.IsFlatRate, a.WorkerId))
            .ToList();
        try
        {
            var input = new JobEditInput(JeAddress, assignments, date, JeNotes, JeServcode, JeSitename, JeWorkorder);
            if (_editingJobId is { } id)
            {
                (await _adminApi.UpdateJobApiAsync(id, input, CancellationToken.None)).Ok();
                JeStatus = $"Updated job #{id}.";
            }
            else
            {
                var result = (await _adminApi.CreateJobApiAsync(input, CancellationToken.None)).Created();
                JeStatus = result is not null ? $"Created job #{result.JobId}." : "Failed.";
                if (result is not null)
                {
                    _editingJobId = result.JobId;
                    await LoadJobForEdit(result.JobId);
                }
            }
            await FetchJobs();
            if (_editingJobId is { } editId)
                HighlightByJobId(editId);
        }
        catch (Exception ex) { JeStatus = $"Error: {ex.Message}"; }
    }

    [RelayCommand]
    private async Task DeleteJob()
    {
        if (_editingJobId is not { } id) return;
        try
        {
            (await _adminApi.DeleteJobApiAsync(id, CancellationToken.None)).Ok();
            _editingJobId = null;
            RightPanel = RightPanelMode.None;
            await FetchJobs();
        }
        catch (Exception ex) { JeStatus = $"Error: {ex.Message}"; }
    }

    // ══════ HELPERS ═════════════════════════════════════════

    private static string? NullIfEmpty(string s) => string.IsNullOrWhiteSpace(s) ? null : s;
    private static Option<string?> Opt(string s) => string.IsNullOrWhiteSpace(s) ? default : new Option<string?>(s);
    private static float? ParseFloat(string s) => float.TryParse(s, out var v) ? v : null;
    private static long? ParseCents(string s) => decimal.TryParse(s, out var v) ? (long?)(v * 100) : null;
    private static string FormatAssignmentData(FullAssignmentData d) =>
        $"Site: {d.Sitename}  WO: {d.Workorder}\nAddr: {d.Address}  Date: {d.Date}\n" +
        $"Miles: {d.MilesDriven}  Hours: {d.HoursDriven}  Min: {d.MinutesDriven}\n" +
        $"Flat rate: {d.UsingFlatRate}  Expenses: {d.Extraexpcents}¢\n" +
        $"Sign in: {d.Signin ?? "-"}  Sign out: {d.Signout ?? "-"}\n" +
        $"Job notes: {d.JobNotes}\nWorker notes: {d.WorkerNotes}";
}