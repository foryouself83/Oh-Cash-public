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
<<<<<<< HEAD
=======
>>>>>>> 8b539a1 (Feat: 미션 완료 적립 API 추가)

        public MissionParticipationRequest()
        {

        }

        public MissionParticipationRequest(int missionSeqNo, string? missionId, string? userKey, string? mac)
        {
            MissionSeqNo = missionSeqNo;
            MissionId = missionId;
            UserKey = userKey;
            Mac = mac;
        }
<<<<<<< HEAD
=======
>>>>>>> ba01ad0 (Feat: 미션 완료 적립 API 추가 진행 #1)
=======
>>>>>>> 8b539a1 (Feat: 미션 완료 적립 API 추가)
    }
}