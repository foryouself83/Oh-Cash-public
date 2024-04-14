using System.Globalization;
using System.Security;
using System.Windows.Data;

namespace PopupCash.Controls.Presentation.Converters
{
    public class PasswordInputToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SecureString password)
            {
                if (password.Length > 0)
                    return true;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
