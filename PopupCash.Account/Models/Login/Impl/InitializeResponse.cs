namespace PopupCash.Account.Models.Login.Impl
{
    public class InitializeResponse
    {

        /// <summary>
        /// 결과 코드
        /// </summary>
        public int Result { get; set; }

        /// <summary>
        /// Database password
        /// </summary>
        public string? Db_password { get; set; }
        /// <summary>
        /// Message
        /// </summary>
        public string? Msg { get; set; }
    }
}
