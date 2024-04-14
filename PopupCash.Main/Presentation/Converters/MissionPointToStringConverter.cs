using System.Globalization;
using System.Windows.Data;
using PopupCash.Core.Extensions;

namespace PopupCash.Main.Presentation.Converters
{
    public class MissionPointToStringConverter : IValueConverter
    {
        private const string postString = "P";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int point)
            {
                return $"{point.FormatWithCommas()}{postString}";
            }

            return $"0{postString}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
