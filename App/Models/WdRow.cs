using CommunityToolkit.Mvvm.ComponentModel;

namespace App.Models;

public partial class WdRow : ObservableObject
{
    public Org.OpenAPITools.Model.WDEntry Data { get; }
    public int Index { get; }

    public bool IsTotal { get; }

    public WdRow(Org.OpenAPITools.Model.WDEntry data, int index, bool isTotal = false)
    {
        Data = data;
        Index = index;
        IsTotal = isTotal;
    }
}