using Newtonsoft.Json;

namespace PopupCash.Account.Models.Socials.Kakaos
{
    public class KakaoUnlinkResponse : KakaoKapiResponse
    {
        [JsonProperty("id")]
        public long? Id { get; set; }
    }
}
