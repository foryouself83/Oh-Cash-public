namespace PopupCash.Account.Models.Authenthications.Impl
{
    public class AuthTokenInfo
    {
        public string? IdToken { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }

        //public AuthTokenInfo(string idToken, string accessToken, string refreshToken)
        //{
        //    IdToken = idToken;
        //    AccessToken = accessToken;
        //    RefreshToken = refreshToken;
        //}
    }
}
