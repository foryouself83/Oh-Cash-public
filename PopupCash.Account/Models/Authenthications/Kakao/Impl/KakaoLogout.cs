using Newtonsoft.Json;

namespace PopupCash.Account.Models.Authenthications.Kakao.Impl
{
    public class KakaoLogout
    {
        [JsonProperty("id")]
        public long Id { get; set; }
    }
}
