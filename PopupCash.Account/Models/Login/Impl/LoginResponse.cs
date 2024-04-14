namespace PopupCash.Account.Models.Login.Impl
{
    /// <summary>
    /// loginUsingPOST api response
    /// </summary>
    public class LoginResponse
    {
        /// <summary>
        /// 결과 코드
        /// </summary>
        public int Result { get; set; }
        /// <summary>
        /// 로그인 여부 (Y/N)
        /// </summary>
        public string? Login { get; set; }
        /// <summary>
        /// Access token
        /// </summary>
        public string? Token { get; set; }
        /// <summary>
        /// Message
        /// </summary>
        public string? Msg { get; set; }
    }
}
