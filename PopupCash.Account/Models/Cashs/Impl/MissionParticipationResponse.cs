using Newtonsoft.Json;

namespace PopupCash.Account.Models.Cashs.Impl
{
    public class MissionParticipationResponse
    {
        [JsonProperty("result")]
        public int Result { get; set; }
        [JsonProperty("participation_seq")]
        public string? ParticipationSeq { get; set; }

        [JsonProperty("msg")]
        public string? Msg { get; set; }
    }
}