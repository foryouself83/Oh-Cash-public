using Newtonsoft.Json;

namespace PopupCash.Account.Models.Cashs.Impl
{
    public class MissionParticipationRequest
    {
        [JsonProperty("mission_seq")]
        public int MissionSeqNo { get; set; }
        [JsonProperty("mission_id")]
        public string? MissionId { get; set; }

        [JsonProperty("media_user_ad_id")]
        public string? Mac { get; set; }
        [JsonProperty("media_user_key")]
        public string? UserKey { get; set; }

        public MissionParticipationRequest()
        {

        }

        public MissionParticipationRequest(int missionSeqNo, string? missionId, string? userKey)
        {
            MissionSeqNo = missionSeqNo;
            MissionId = missionId;
            UserKey = userKey;
        }
    }
}