using System.Globalization;

namespace Host;

public class BooleanToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (bool)value ? "Stop" : "Play";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (string)value == "Stop";
    }
}
