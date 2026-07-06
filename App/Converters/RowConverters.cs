using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Avalonia.Styling;
using Org.OpenAPITools.Model;

namespace App.Converters;

public class StripeConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int index)
            return index % 2 == 1
                ? new SolidColorBrush(Color.Parse("#0A000000"))
                : Brushes.Transparent;
        return Brushes.Transparent;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}

public class HighlightConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is true)
            return new SolidColorBrush(Color.Parse("#FF3B82F6"));
        return Brushes.Transparent;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}

public class UnassignedConverter : IValueConverter
{
    private static readonly SolidColorBrush Red = new(Color.Parse("#FFDC2626"));

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null or 0L)
            return Red;
        return Brushes.Black;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}

public class StatusColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var color = (value as Status?) switch
        {
            Status.Assigned => "#FF22C55E",
            Status.Started => "#FFF97316",
            Status.Signedout => "#FFEF4444",
            Status.Outnotin => "#FF3B82F6",
            _ => "#FF000000"
        };
        return new SolidColorBrush(Color.Parse(color));
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}

public class StatusEmojiConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return (value as Status?) switch
        {
            Status.Assigned => "\u21A3",
            Status.Started => "\u23F3",
            Status.Signedout => "\u2714",
            Status.Outnotin => "\u2047",
            _ => ""
        };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}

public class IncompleteColorConverter : IValueConverter
{
    private static readonly SolidColorBrush Red = new(Color.Parse("#FFDC2626"));
    private static readonly IBrush LightFg = Brushes.Black;
    private static readonly IBrush DarkFg = Brushes.White;

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is false)
            return Red;
        return Application.Current?.ActualThemeVariant == ThemeVariant.Dark ? DarkFg : LightFg;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
