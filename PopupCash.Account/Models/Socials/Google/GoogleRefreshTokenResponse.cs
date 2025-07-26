using Newtonsoft.Json;

namespace PopupCash.Account.Models.Socials.Google
{
    public class GoogleRefreshTokenResponse
    {
        // 사용자 액세스 토큰 값
        [JsonProperty("access_token")]
        public string? AccessToken { get; set; }

        // 액세스 토큰과 ID 토큰의 만료 시간
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        // 인증된 사용자의 정보 조회 권한 범위 범위가 여러 개일 경우, 공백으로 구분
        // OpenID Connect가 활성화된 앱의 토큰 발급 요청인 경우, ID 토큰이 함께 발급되며 scope 값에 openid 포함
        [JsonProperty("scope")]
        public string? Scope { get; set; }
        // 토큰 타입, bearer로 고정
        [JsonProperty("token_type")]
        public string? TokenType { get; set; }
    }
}
