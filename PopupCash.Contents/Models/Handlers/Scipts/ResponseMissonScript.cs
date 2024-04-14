using Newtonsoft.Json;

namespace PopupCash.Contents.Models.Handlers.Scipts
{
    public class ResponseMissonScript
    {
        [JsonProperty("src")]
        public string? Src { get; set; }
        [JsonProperty("mission_seq")]
        public int MissionSeq { get; set; }
        [JsonProperty("mission_id")]
        public string? MissionId { get; set; }
        [JsonProperty("adver_name")]
        public string? AdverName { get; set; }
        [JsonProperty("adver_url")]
        public string? AdverUrl { get; set; }
        [JsonProperty("keyword")]
        public string? Keyword { get; set; }
        [JsonProperty("user_point")]
        public int UserPoint { get; set; }
        [JsonProperty("mission_class")]
        public string? MissionClass { get; set; }
        [JsonProperty("check_url")]
        public string? CheckUrl { get; set; }
        [JsonProperty("checkurl2")]
        public string? CheckUrl2 { get; set; }
        [JsonProperty("checkurl3")]
        public string? CheckUrl3 { get; set; }
        [JsonProperty("check_time")]
        public int CheckTime { get; set; }
        [JsonProperty("p_search_nm")]
        public string? Search_nm { get; set; }
        [JsonProperty("p_no")]
        public string? No { get; set; }
        [JsonProperty("pno2")]
        public string? No2 { get; set; }
        [JsonProperty("pno3")]
        public string? No3 { get; set; }
        [JsonProperty("p_syncnvmid")]
        public string? Syncnvmid { get; set; }
    }



    //    public class ResponseMissonScript
    //    {
    //#pragma warning disable IDE1006 // 명명 스타일

    //        [JsonProperty("src")]
    //        public string? src { get; set; }
    //        [JsonProperty("mission_seq")]
    //        public int mission_seq { get; set; }
    //        [JsonProperty("mission_id")]
    //        public string? mission_id { get; set; }
    //        [JsonProperty("adver_name")]
    //        public string? adver_name { get; set; }
    //        [JsonProperty("adver_url")]
    //        public string? adver_url { get; set; }
    //        [JsonProperty("keyword")]
    //        public string? keyword { get; set; }
    //        [JsonProperty("user_point")]
    //        public int user_point { get; set; }
    //        [JsonProperty("mission_class")]
    //        public string? mission_class { get; set; }
    //        [JsonProperty("check_url")]
    //        public string? check_url { get; set; }
    //        [JsonProperty("check_time")]
    //        public int check_time { get; set; }
    //        [JsonProperty("p_search_nm")]
    //        public string? p_search_nm { get; set; }
    //        [JsonProperty("p_no")]
    //        public string? p_no { get; set; }
    //        [JsonProperty("p_syncnvmid")]
    //        public string? p_syncnvmid { get; set; }
    //#pragma warning restore IDE1006 // 명명 스타일
    //    }

}
