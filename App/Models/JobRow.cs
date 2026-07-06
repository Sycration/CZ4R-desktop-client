using CommunityToolkit.Mvvm.ComponentModel;

namespace App.Models;

public partial class JobRow : ObservableObject
{
    public Org.OpenAPITools.Model.JobData Data { get; }
    public int Index { get; }
    public bool IsUnassigned => Data.WorkerId is null or 0;

    [ObservableProperty]
    private bool _isHighlighted;

    public JobRow(Org.OpenAPITools.Model.JobData data, int index)
    {
        Data = data;
        Index = index;
    }
}