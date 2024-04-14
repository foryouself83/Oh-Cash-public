namespace PopupCash.Core.Extensions
{
    public static class NumberFormattingExtensions
    {
        public static string FormatWithCommas(this int number)
        {
            return string.Format("{0:n0}", number); // 정수를 3자리 마다 쉼표로 구분된 문자열로 변환
        }

        public static string FormatWithCommas(this string number)
        {
            if (int.TryParse(number, out int intValue))
            {
                return intValue.FormatWithCommas();
            }
            else
            {
                return number; // 정수로 변환할 수 없는 경우에는 원래 문자열 반환
            }
        }
    }

}
