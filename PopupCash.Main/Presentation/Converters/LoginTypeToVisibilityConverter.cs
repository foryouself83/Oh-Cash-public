using System.Globalization;
using System.Windows;
using System.Windows.Data;
using PopupCash.Core.Models.Constants;

namespace PopupCash.Main.Presentation.Converters
{
    public class LoginTypeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string LoginType && !LoginType.Equals(ConstantString.AppType))
            {
                return Visibility.Collapsed;
            }

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
