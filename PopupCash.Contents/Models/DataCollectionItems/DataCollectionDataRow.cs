namespace PopupCash.Contents.Models.DataCollectionItems
{
    /// <summary>
    /// 정보 수집 윈도우에서 사용하는 아이템 클래스
    /// </summary>
    public class DataCollectionDataRow
    {
        public string Name { get; init; }
        public string Description { get; init; }

        public DataCollectionDataRow(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
