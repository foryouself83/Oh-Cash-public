namespace PopupCash.Database.Models.Users
{

    public class Authorization
    {
        /// <summary>
        /// 가입 유형(0: 힐링캐시, 1:구글, 2: 페이스북, 3: 카카오, 4: 네이버)
        /// </summary>
        public string? Type { get; set; }
        /// <summary>
        /// rest api 시 사용하는 Key 값
        /// </summary>
        public string? Key { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
