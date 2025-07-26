using Newtonsoft.Json;

namespace PopupCash.Account.Models.Login.Impl
{
    public class InitializeResponse
    {

        /// <summary>
        /// 결과 코드
        /// </summary>
        [JsonProperty("result")]
        public int Result { get; set; }

        [JsonProperty("kakao")]
        public KakaoRestKey? KakaoRestKey { get; set; }

        [JsonProperty("naver")]
        public NaverRestKey? NaverRestKey { get; set; }

        [JsonProperty("google")]
        public GoogleRestKey? GoogleRestKey { get; set; }

        [JsonProperty("pomission")]
        public PomissionInfo? PomissionInfo { get; set; }

        /// <summary>
        /// Database password
        /// </summary>
        [JsonProperty("db_password")]
        public string? DatabasePassword { get; set; }
        /// <summary>
        /// Message
        /// </summary>
        [JsonProperty("msg")]
        public string? Msg { get; set; }
    }
}
