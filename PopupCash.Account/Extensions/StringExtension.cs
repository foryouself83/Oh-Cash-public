using System.Net.Http;
using System.Text;

namespace PopupCash.Account.Extensions
{
    internal static class StringExtension
    {
        public static StringContent TotJsonTypeStringConten(this string value)
        {
            return new StringContent(value, Encoding.UTF8, "application/json");
        }
    }
}
