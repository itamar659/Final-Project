using System.Globalization;

namespace Client;

/// <summary>
/// Milliseconds to time as string
/// </summary>
public class DoubleMSToTimeStrConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        int seconds = (int)((double)value);
        int minutes = seconds / 60;
        int hours = minutes / 60;

        return string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes % 60, seconds % 60);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
