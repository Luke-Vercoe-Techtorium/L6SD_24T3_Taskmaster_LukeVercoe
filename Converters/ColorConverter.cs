using System.Globalization;

namespace TaskManager.Converters
{
    public class ColorConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var color = value?.ToString();
            return Color.FromArgb(color);
        }

        public object? ConvertBack(object? value, Type target, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
