using Newtonsoft.Json;

namespace PopupCash.Account.Models.Login.Impl
{
    public class PomissionInfo
    {
        [JsonProperty("refresh_token")]
        public string? RefreshToken { get; set; }

        [JsonProperty("media_id")]
        public string? MediaId { get; set; }
    }
}