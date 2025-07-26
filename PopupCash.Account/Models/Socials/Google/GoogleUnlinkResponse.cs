using Newtonsoft.Json;

namespace PopupCash.Account.Models.Socials.Google
{
    public class GoogleUnlinkResponse
    {
        [JsonProperty("id")]
        public long? Id { get; set; }
    }
}
