using Newtonsoft.Json;

namespace PopupCash.Account.Models.Cashs.Impl
{
    public class PomissionAccessTokenResponse
    {
        [JsonProperty("result")]
        public int Result { get; set; }
        [JsonProperty("token")]
        public string? Token { get; set; }

        [JsonProperty("msg")]
        public string? Msg { get; set; }
    }
}