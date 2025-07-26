using Newtonsoft.Json;

namespace PopupCash.Account.Models.Login.Impl
{
    public class GoogleRestKey
    {
        [JsonProperty("client_id")]
        public string? ClientId { get; set; }

        [JsonProperty("client_secret")]
        public string? ClientSecret { get; set; }
        [JsonProperty("redirect_uri")]
        public string? RedirectUri { get; set; }
    }
}