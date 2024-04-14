using System.Globalization;
using System.Windows.Data;

namespace PopupCash.Contents.Presentation.Converters
{
    public class RegDateToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string dateString)
            {
                // 문자열을 DateTime 형식으로 파싱
                DateTime dateTime = DateTime.ParseExact(dateString, "yyyyMMddHHmmss", null);

                // 출력 형식에 맞게 문자열로 변환
                return dateTime.ToString("yyyy.MM.dd HH:mm:ss");
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
