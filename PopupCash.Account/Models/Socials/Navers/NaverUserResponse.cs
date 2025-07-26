using Newtonsoft.Json;

namespace PopupCash.Account.Models.Socials.Navers
{
    public class NaverUserResponse
    {
        /// <summary>
        /// API 호출 결과 코드
        /// </summary>
        [JsonProperty("resultcode")]
        public string? ResultCode { get; set; }

        /// <summary>
        /// 호출 결과 메시지
        /// </summary>
        [JsonProperty("message")]
        public string? Message { get; set; }

        [JsonProperty("response")]
        public ResponseData? Response { get; set; }

        public class ResponseData
        {
            /// <summary>
            /// 동일인 식별 정보
            /// 네이버 아이디마다 고유하게 발급되는 유니크한 일련번호 값
            /// </summary>
            [JsonProperty("id")]
            public string? Id { get; set; }

            /// <summary>
            /// 사용자 별명
            /// (별명이 설정되어 있지 않으면 id*** 형태로 리턴됩니다.)
            /// </summary>
            [JsonProperty("nickname")]
            public string? Nickname { get; set; }

            /// <summary>
            /// 사용자 이름
            /// </summary>
            [JsonProperty("name")]
            public string? Name { get; set; }

            /// <summary>
            /// 사용자 메일 주소
            /// 기본적으로 네이버 내정보에 등록되어 있는 '기본 이메일' 즉 네이버ID@naver.com 값이나,
            /// 사용자가 다른 외부메일로 변경했을 경우는 변경된 이메일 주소로 됩니다.
            /// </summary>
            [JsonProperty("email")]
            public string? Email { get; set; }

            /// <summary>
            /// 성별
            /// - F: 여성
            /// - M: 남성
            /// - U: 확인불가
            /// </summary>
            [JsonProperty("gender")]
            public string? Gender { get; set; }

            /// <summary>
            /// 사용자 연령대
            /// </summary>
            [JsonProperty("age")]
            public string? Age { get; set; }

            /// <summary>
            /// 사용자 생일 (MM-DD 형식)
            /// </summary>
            [JsonProperty("birthday")]
            public string? Birthday { get; set; }

            /// <summary>
            /// 사용자 프로필 사진 URL
            /// </summary>
            [JsonProperty("profile_image")]
            public string? ProfileImage { get; set; }

            /// <summary>
            /// 출생연도
            /// </summary>
            [JsonProperty("birthyear")]
            public string? BirthYear { get; set; }

            /// <summary>
            /// 휴대전화번호
            /// </summary>
            [JsonProperty("mobile")]
            public string? Mobile { get; set; }
        }
    }
}
