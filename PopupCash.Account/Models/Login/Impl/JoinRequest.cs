using Newtonsoft.Json;

namespace PopupCash.Account.Models.Login.Impl
{
    public class JoinRequest
    {
        [JsonProperty("key")]
        public string? Key { get; set; }

        [JsonProperty("adid")]
        public string? AdId { get; set; }

        [JsonProperty("mac")]
        public string? Mac { get; set; }

        [JsonProperty("email")]
        public string? Email { get; set; }

        [JsonProperty("password")]
        public string? Password { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("phone")]
        public string? Phone { get; set; }

        [JsonProperty("birth")]
        public string? Birth { get; set; }

        [JsonProperty("sex")]
        public string? Sex { get; set; }

        [JsonProperty("type")]
        public string? Type { get; set; }

        [JsonProperty("recommender")]
        public string? Recommender { get; set; }

        [JsonProperty("deviceId")]
        public string? DeviceId { get; set; }

        [JsonProperty("deviceModel")]
        public string? DeviceModel { get; set; }

        [JsonProperty("deviceOs")]
        public string? DeviceOs { get; set; }

        [JsonProperty("appVersion")]
        public string? AppVersion { get; set; }
    }

    //#pragma warning disable IDE1006 // 명명 스타일
    //    public class JoinRequest
    //    {
    //        /// <summary>
    //        /// 키
    //        /// </summary>
    //        public string? key { get; set; }

    //        /// <summary>
    //        /// 광고 ID
    //        /// </summary>
    //        public string? adid { get; set; }

    //        /// <summary>
    //        /// PC의 MAC 주소
    //        /// </summary>
    //        public string? mac { get; set; }

    //        /// <summary>
    //        /// 이메일 주소 (아이디)
    //        /// </summary>
    //        public string? email { get; set; }

    //        /// <summary>
    //        /// 비밀번호
    //        /// </summary>
    //        public string? password { get; set; }

    //        /// <summary>
    //        /// 사용자 이름
    //        /// </summary>
    //        public string? name { get; set; }

    //        /// <summary>
    //        /// 핸드폰 번호
    //        /// </summary>
    //        public string? phone { get; set; }

    //        /// <summary>
    //        /// 생년월일 (YYYYMMDD)
    //        /// </summary>
    //        public string? birth { get; set; }

    //        /// <summary>
    //        /// 성별 (1: 남자, 2: 여자)
    //        /// </summary>
    //        public string? sex { get; set; }

    //        /// <summary>
    //        /// 가입 유형 (0: 팝업캐시, 1: 구글, 2: 페이스북, 3: 카카오, 4: 네이버)
    //        /// </summary>
    //        public string? type { get; set; }

    //        /// <summary>
    //        /// 추천인 코드
    //        /// </summary>
    //        public string? recommender { get; set; }

    //        /// <summary>
    //        /// 디바이스 아이디
    //        /// </summary>
    //        public string? deviceId { get; set; }

    //        /// <summary>
    //        /// 디바이스 모델
    //        /// </summary>
    //        public string? deviceModel { get; set; }

    //        /// <summary>
    //        /// 디바이스 OS
    //        /// </summary>
    //        public string? deviceOs { get; set; }

    //        /// <summary>
    //        /// 앱 버전
    //        /// </summary>
    //        public string? appVersion { get; set; }
    //    }
    //#pragma warning restore IDE1006 // 명명 스타일
}
