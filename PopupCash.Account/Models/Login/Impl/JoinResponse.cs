using Newtonsoft.Json;

namespace PopupCash.Account.Models.Login.Impl
{
    public class JoinResponse
    {

        /// <summary>
        /// 결과 코드
        /// </summary>
        [JsonProperty("result")]
        public int Result { get; set; }
        /// <summary>
        /// 로그인 여부 (Y/N)
        /// </summary>
        [JsonProperty("login")]
        public string? Login { get; set; }
        /// <summary>
        /// Access token
        /// </summary>
        [JsonProperty("token")]
        public string? Token { get; set; }
        /// <summary>
        /// 추천인 적립 여부 (Y/N)
        /// </summary>
        [JsonProperty("recommand")]
        public string? Recommand { get; set; }

        /// <summary>
        /// key
        /// </summary>
        [JsonProperty("key")]
        public string? Key { get; set; }

        /// <summary>
        /// 가입 유형 (0: 팝업캐시, 1: 구글, 2: 페이스북, 3: 카카오, 4: 네이버)
        /// </summary>
        [JsonProperty("type")]
        public string? Type { get; set; }
        /// <summary>
        /// Message
        /// </summary>
        [JsonProperty("msg")]
        public string? Msg { get; set; }
    }
}
