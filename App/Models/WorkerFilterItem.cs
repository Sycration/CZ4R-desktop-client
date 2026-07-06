using CommunityToolkit.Mvvm.ComponentModel;

namespace App.Models;

public partial class WorkerFilterItem : ObservableObject
{
    public long Id { get; }
    public string Name { get; }

    [ObservableProperty]
    private bool _isSelected;

    public WorkerFilterItem(long id, string name, bool selected)
    {
        Id = id;
        Name = name;
        _isSelected = selected;
    }
}