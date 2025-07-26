using Newtonsoft.Json;

namespace PopupCash.Account.Models.Socials.Navers
{
    public class NaverUnlinkResponse
    {
        [JsonProperty("access_token")]
        public string? AccessToken { get; set; }

        [JsonProperty("result")]
        public string? Result { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("error")]
        public string? Error { get; set; }

        [JsonProperty("error_description")]
        public string? ErrorDescription { get; set; }
    }
}
