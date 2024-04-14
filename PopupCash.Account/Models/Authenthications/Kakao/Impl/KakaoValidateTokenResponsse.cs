using Newtonsoft.Json;

namespace PopupCash.Account.Models.Authenthications.Kakao.Impl
{
    public class KakaoValidateTokenResponsse
    {
        //	회원번호
        [JsonProperty("id")]
        public long Id { get; set; }
        // 액세스 토큰 만료 시간(초)

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
        // 토큰이 발급된 앱 ID
        [JsonProperty("app_id")]
        public int AppId { get; set; }

        public KakaoValidateTokenResponsse()
        {
            Id = 0;
            ExpiresIn = 0;
            AppId = 0;
        }
    }
}
