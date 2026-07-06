using CommunityToolkit.Mvvm.ComponentModel;

namespace App.Models;

public partial class AssignmentRow : ObservableObject
{
    public long WorkerId { get; }
    public string WorkerName { get; }

    [ObservableProperty]
    private bool _isAssigned;

    [ObservableProperty]
    private bool _isFlatRate;

    public AssignmentRow(long workerId, string workerName, bool assigned, bool flatRate)
    {
        WorkerId = workerId;
        WorkerName = workerName;
        _isAssigned = assigned;
        _isFlatRate = flatRate;
    }

    partial void OnIsAssignedChanged(bool value)
    {
        if (!value)
            IsFlatRate = false;
    }
}