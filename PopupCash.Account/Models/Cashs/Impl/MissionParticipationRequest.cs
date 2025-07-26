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
<<<<<<< HEAD

        public MissionParticipationRequest()
        {

        }

        public MissionParticipationRequest(int missionSeqNo, string? missionId, string? userKey)
        {
            MissionSeqNo = missionSeqNo;
            MissionId = missionId;
            UserKey = userKey;
        }
=======
>>>>>>> ba01ad0 (Feat: 미션 완료 적립 API 추가 진행 #1)
    }
}