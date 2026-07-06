using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using App.ViewModels;

namespace App.Views;

public partial class AdminView : UserControl
{
    public AdminView()
    {
        InitializeComponent();
    }

    private async void ExportDatabase_Click(object? sender, RoutedEventArgs e)
    {
        if (DataContext is not AdminViewModel vm) return;

        vm.ExportStatus = "Exporting...";
        var bytes = await vm.ExportDatabaseAsync();

        if (bytes is not { Length: > 0 }) return;

        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel is null) return;

        var file = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Save Database Export",
            DefaultExtension = "sql",
            SuggestedFileName = $"cz4r-export-{DateTime.Now:yyyyMMdd-HHmmss}.sql",
            FileTypeChoices = [new FilePickerFileType("SQL Dump") { Patterns = ["*.sql"] }]
        });

        if (file is null) return;

        await using var stream = await file.OpenWriteAsync();
        await stream.WriteAsync(bytes);
        vm.ExportStatus = $"Saved to {file.Name}";
    }

    private bool _wdDateRangeInitializing;

    private void WdDateRangeCalendar_Loaded(object? sender, RoutedEventArgs e)
    {
        var today = DateTime.Today;
        var firstOfMonth = new DateTime(today.Year, today.Month, 1);
        _wdDateRangeInitializing = true;
        WdDateRangeCalendar.SelectedDates.AddRange(firstOfMonth, today);
        _wdDateRangeInitializing = false;
    }

    private void WdDateRange_SelectedDatesChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (_wdDateRangeInitializing || DataContext is not AdminViewModel vm) return;
        var dates = WdDateRangeCalendar.SelectedDates.OrderBy(d => d).ToArray();
        vm.WdStartDate = dates.Length > 0 ? dates[0] : null;
        vm.WdEndDate = dates.Length > 1 ? dates[^1] : null;
    }
}