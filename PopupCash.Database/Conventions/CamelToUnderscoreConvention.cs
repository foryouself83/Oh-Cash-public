using System.Text.RegularExpressions;
using Dapper.FluentMap.Conventions;

namespace PopupCash.Database.Conventions
{
    /// <summary>
    /// Camel case to underscore seperated case
    /// This configuration will map camel case property names to underscore seperated database column names
    /// ex) UrlOptimizedName -> Url_Optimized_Name
    /// </summary>
    public class CamelToUnderscoreConvention : Convention
    {
        public CamelToUnderscoreConvention()
        {

            Properties()
                .Configure(c => c.Transform(s => ConvertToCamelAndUnderscore(s)).IsCaseInsensitive());
        }
        public static string ConvertToCamelAndUnderscore(string value)
        {
            return Regex.Replace(input: value, pattern: "([A-Z])([A-Z][a-z])|([a-z0-9])([A-Z])", replacement: "$1$3_$2$4");
        }
    }
}
