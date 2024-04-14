namespace PopupCash.Account.Models.Users.Impl
{
    /// <summary>
    /// getUserUsingGET api response
    /// </summary>
    public class UserResponse
    {
        public int Result { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? Profile { get; set; }
        public string? Cash { get; set; }
        public string? Grade { get; set; }
        public int Mission_point { get; set; }
        public string? Flag { get; set; }
        /// <summary>
        /// 미션 리스트 URI 사용
        /// </summary>
        public string? Mac { get; set; }
        /// <summary>
        /// 미션 리스트 URI 사용
        /// </summary>
        public string? Key { get; set; }
        public Say? Saying { get; set; }
        public Notice? Notice { get; set; }
        public Limit? Limit { get; set; }

        public string? msg { get; set; }
    }
    public class Say
    {
        public string? Saying { get; set; }
        public string? Scrap { get; set; }
        public string? Like { get; set; }
    }

    public class Notice
    {
        public int Pk { get; set; }
        public string? Title { get; set; }
        public string? New_yn { get; set; }
    }

    public class Limit
    {
        public string? Limit_yn { get; set; }
        public string? Limit_s_date { get; set; }
        public string? Limit_e_date { get; set; }
    }


}
