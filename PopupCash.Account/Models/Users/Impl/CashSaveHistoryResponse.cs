namespace PopupCash.Account.Models.Users.Impl
{
    public class CashSaveHistoryResponse
    {
        public int Result { get; set; }
        public string? Total { get; set; }
        public string? Expire { get; set; }
        public SaveData? Save { get; set; }
    }

    public class SaveData
    {
        public string? Gem { get; set; }
        public string? Like { get; set; }
        public string? Recommender { get; set; }
        public string? Event { get; set; }
        public string? Reward { get; set; }
        public List<CashData>? Cash { get; set; }
    }

    public class CashData
    {
        public string? Comment { get; set; }
        public string? Reg_Date { get; set; }
        public string? Cash { get; set; }
        public string? Type { get; set; }
    }
}
