using PopupCash.Core.Models.Constants;

namespace PopupCash.Core.Extensions
{
    public static class StringExtension
    {
        public static string GetJoinType(this string joinType)
        {
            switch (joinType)
            {
                case "0":
                    return ConstantString.AppName;
                case "1":
                    return ConstantString.GoogleName;
                case "2":
                    return ConstantString.FaceBookName;
                case "3":
                    return ConstantString.KakaoName;
                case "4":
                    return ConstantString.NaverName;
                default:
                    return string.Empty;
            }
        }
    }
}
