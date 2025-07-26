using Newtonsoft.Json;

namespace PopupCash.Account.Models.Socials.Kakaos
{
    public class KakaoKauthResponse
    {
        [JsonProperty("error")]
        public string? Error { get; set; }

        [JsonProperty("error_description")]
        public string? ErrorDescription { get; set; }
    }
}
