using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using App.Models;
using App.ViewModels;

namespace App.Views;

public partial class DashboardView : UserControl
{
    public DashboardView()
    {
        InitializeComponent();
    }

    private bool _dateRangeInitializing;

    private void DateRangeCalendar_Loaded(object? sender, RoutedEventArgs e)
    {
        var today = DateTime.Today;
        _dateRangeInitializing = true;
        DateRangeCalendar.SelectedDates.AddRange(today, today.AddDays(15));
        _dateRangeInitializing = false;
    }

    private void DateRange_SelectedDatesChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (_dateRangeInitializing || DataContext is not DashboardViewModel vm) return;
        var dates = DateRangeCalendar.SelectedDates.OrderBy(d => d).ToArray();
        vm.JlStartDate = dates.Length > 0 ? dates[0] : null;
        vm.JlEndDate = dates.Length > 1 ? dates[^1] : null;
    }

    private void OpenJob_Click(object? sender, RoutedEventArgs e)
    {
        if (DataContext is not DashboardViewModel vm) return;
        if (sender is Button btn && btn.DataContext is JobRow row)
        {
            vm.SelectedJob = row.Data;
            vm.HighlightSingle(row.Data.JobId, row.Data.WorkerId);
            vm.SwitchToDetail();
        }
    }

    private void EditJob_Click(object? sender, RoutedEventArgs e)
    {
        if (DataContext is not DashboardViewModel vm) return;
        if (sender is Button btn && btn.DataContext is JobRow row)
        {
            if (vm.EditorVisible && vm.EditingJobId == row.Data.JobId) return;
            vm.SelectedJob = row.Data;
            vm.BeginEditJob(row.Data);
        }
    }
}