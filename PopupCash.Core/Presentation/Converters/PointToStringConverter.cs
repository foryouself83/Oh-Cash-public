using System.Globalization;
using System.Windows.Data;

namespace PopupCash.Core.Presentation.Converters
{
    public class PointToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int intValue)
            {
                string formattedValue = intValue.ToString("N0", culture); // 숫자에 쉼표 추가
                return $"{formattedValue}P";
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
