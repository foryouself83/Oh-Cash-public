namespace PopupCash.Main.Models.Joins
{
    public class IdentityVerificationResponse
    {
        /// <summary>
        /// 이름
        /// </summary>
        public string? RSLT_NAME { get; set; }
        /// <summary>
        /// 생년 월일
        /// </summary>
        public string? RSLT_BIRTHDAY { get; set; }
        /// <summary>
        /// 성별
        /// </summary>
        public string? RSLT_SEX_CD { get; set; }
        /// <summary>
        /// 휴대폰 번호
        /// </summary>
        public string? TEL_NO { get; set; }
    }
}
