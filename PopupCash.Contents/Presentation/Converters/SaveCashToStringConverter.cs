using System.Globalization;
using System.Windows.Data;
using PopupCash.Core.Extensions;

namespace PopupCash.Contents.Presentation.Converters
{
    public class SaveCashToStringConverter : IValueConverter
    {
        private const string postString = "P";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string cashString)
            {
                if (double.TryParse(cashString, out var cash))
                {
                    return $"{((int)cash).FormatWithCommas()}{postString}";
                }
            }

            return $"0{postString}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
