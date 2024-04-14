using System.Globalization;
using System.Windows.Data;
using PopupCash.Core.Extensions;

namespace PopupCash.Main.Presentation.Converters
{
    public class CashToStringConverter : IValueConverter
    {
        private const string preString = "보유포인트 : ";
        private const string postString = "P";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string cash)
            {
                return $"{preString}{cash.FormatWithCommas()}{postString}";
            }

            return $"{preString}0{postString}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
