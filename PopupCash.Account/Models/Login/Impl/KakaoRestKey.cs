using Newtonsoft.Json;

namespace PopupCash.Account.Models.Login.Impl
{
    public class KakaoRestKey
    {
        [JsonProperty("rest_api_key")]
        public string? RestApiKey { get; set; }

        [JsonProperty("client_secret")]
        public string? ClientSecret { get; set; }
        [JsonProperty("redirect_uri")]
        public string? RedirectUri { get; set; }
    }
}