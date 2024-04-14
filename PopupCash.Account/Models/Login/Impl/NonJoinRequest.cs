namespace PopupCash.Account.Models.Login.Impl
{
    using Newtonsoft.Json;

    public class NonJoinRequest
    {
        [JsonProperty("adid")]
        public string? AdId { get; set; }

        [JsonProperty("mac")]
        public string? Mac { get; set; }

        [JsonProperty("non")]
        public string? Non { get; set; }
    }


    //#pragma warning disable IDE1006 // 명명 스타일
    //    public class NonJoinRequest
    //    {
    //        /// <summary>
    //        /// 광고 ID
    //        /// </summary>
    //        public string? adid { get; set; }
    //        /// <summary>
    //        /// PC의 MAC 주소
    //        /// </summary>
    //        public string? mac { get; set; }

    //        /// <summary>
    //        /// 비회원 이용 구분 (Y;/N)
    //        /// </summary>
    //        public string? non { get; set; }
    //    }
    //#pragma warning restore IDE1006 // 명명 스타일

}
