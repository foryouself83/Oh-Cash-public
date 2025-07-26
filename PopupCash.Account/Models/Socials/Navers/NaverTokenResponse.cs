using Newtonsoft.Json;

namespace PopupCash.Account.Models.Socials.Navers
{
    public class NaverTokenResponse
    {
        [JsonProperty("access_token")]
        public string? AccessToken { get; set; }

        [JsonProperty("refresh_token")]
        public string? RefreshToken { get; set; }

        [JsonProperty("token_type")]
        public string? TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("error")]
        public string? Error { get; set; }

        [JsonProperty("error_description")]
        public string? ErrorDescription { get; set; }
    }
}
