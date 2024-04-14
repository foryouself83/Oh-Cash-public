using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PopupCash.Core.Presentation.Converters
{
    public class BooleanToVisibilityCollapsedConverter : IValueConverter
    {
        // True를 False로, False를 True로 변환
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue && boolValue)
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        // 변환된 값을 다시 원래 값으로 되돌릴 필요가 없으므로 구현하지 않음
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
