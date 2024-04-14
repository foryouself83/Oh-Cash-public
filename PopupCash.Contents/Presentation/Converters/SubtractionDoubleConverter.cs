using System.Globalization;
using System.Windows.Data;

namespace PopupCash.Contents.Presentation.Converters
{
    /// <summary>
    /// value에서 parameter를 빼는 Conveter
    /// value는 double 타입만 지원
    /// </summary>
    internal class SubtractionDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double originValue && parameter is string subtractionString)
            {
                if (double.TryParse(subtractionString, out var subtractionValue))
                    return originValue - subtractionValue;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
