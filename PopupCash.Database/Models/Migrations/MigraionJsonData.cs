namespace PopupCash.Database.Models.Migrations
{
    public class MigraionJsonDataList
    {
        public List<MigraionJsonData>? MigraionList { get; set; }

    }

    public class MigraionJsonData
    {
        /// <summary>
        /// 마이그레이션의 버전 정보
        /// </summary>
        public float Version { get; set; }

        public List<string>? Queries { get; set; }

        /// <summary>
        /// 업데이트 일자
        /// </summary>
        public DateTime? UpdateDate { get; set; }
    }
}
