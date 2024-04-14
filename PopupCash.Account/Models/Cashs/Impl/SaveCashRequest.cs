using Newtonsoft.Json;

namespace PopupCash.Account.Models.Cashs.Impl
{
    public class SaveCashRequest
    {
        [JsonProperty("cash")]
        public string? Cash { get; set; }
        [JsonProperty("type")]
        public string? Type { get; set; }
        [JsonProperty("code")]
        public string? Code { get; set; }
        [JsonProperty("comment")]
        public string? Comment { get; set; }
        [JsonProperty("message_id")]
        public string? MessageId { get; set; }
        [JsonProperty("mission_id")]
        public string? MissionId { get; set; }
        [JsonProperty("mission_seq")]
        public string? MissionSeq { get; set; }

        public SaveCashRequest(string? cash, string? comment, string? mission_id, string? mission_seq)
        {
            Cash = cash;
            Comment = comment;
            MissionId = mission_id;
            MissionSeq = mission_seq;
        }
    }
}