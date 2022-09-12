using System.Globalization;

namespace Host;

public class BooleanToStringConverter : IValueConverter
{
    public string ValTrue { get; set; }
    public string ValFalse { get; set; }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (bool)value ? ValTrue : ValFalse;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (string)value == ValTrue;
    }
}
