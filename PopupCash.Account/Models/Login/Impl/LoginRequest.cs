namespace PopupCash.Account.Models.Login.Impl
{
    using Newtonsoft.Json;

    public class LoginRequest
    {
        /// <summary>
        /// KEY
        /// </summary>
        [JsonProperty("key")]
        public string? Key { get; set; }

        /// <summary>
        /// ADID
        /// </summary>
        [JsonProperty("adid")]
        public string? AdId { get; set; }

        /// <summary>
        /// MAC 주소
        /// </summary>
        [JsonProperty("mac")]
        public string? Mac { get; set; }

        /// <summary>
        /// 이메일(아이디)
        /// </summary>
        [JsonProperty("email")]
        public string? Email { get; set; }

        /// <summary>
        /// 비밀번호
        /// </summary>
        [JsonProperty("password")]
        public string? Password { get; set; }

        /// <summary>
        /// 가입 유형(0: 힐링캐시, 1:구글, 2: 페이스북, 3: 카카오, 4: 네이버)
        /// </summary>
        [JsonProperty("type")]
        public string? Type { get; set; }
    }
}
