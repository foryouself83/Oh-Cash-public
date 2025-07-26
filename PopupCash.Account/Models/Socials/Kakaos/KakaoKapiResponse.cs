using Newtonsoft.Json;

namespace PopupCash.Account.Models.Socials.Kakaos
{
    public class KakaoKapiResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }
        [JsonProperty("msg")]
        public string? Message { get; set; }
    }
}
