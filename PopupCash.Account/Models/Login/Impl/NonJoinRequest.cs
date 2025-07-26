namespace PopupCash.Account.Models.Login.Impl
{
    using Newtonsoft.Json;

    public class NonJoinRequest
    {
        [JsonProperty("adid")]
        public string? AdId { get; set; }

        [JsonProperty("mac")]
        public string? Mac { get; set; }

        [JsonProperty("non")]
        public string? Non { get; set; }
    }
}
