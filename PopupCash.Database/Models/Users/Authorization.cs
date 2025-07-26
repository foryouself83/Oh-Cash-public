namespace PopupCash.Database.Models.Users
{

    public class Authorization
    {
        /// <summary>
        /// 가입 유형(0: 힐링캐시, 1:구글, 2: 페이스북, 3: 카카오, 4: 네이버)
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 비회원 Key
        /// </summary>
        public string? Key { get; set; }
        /// <summary>
        /// 회원 조회 key / Pomission 에서 사용
        /// </summary>
        public string? PomissionKey { get; set; }
        public string AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public bool Policy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public Authorization()
        {
            Type = string.Empty;
            AccessToken = string.Empty;
            Policy = false;
        }
    }
}
