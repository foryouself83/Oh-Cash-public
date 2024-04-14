using PopupCash.Account.Models.Users.Impl;

namespace PopupCash.Contents.Models.Cashs
{
    public class CashHistory
    {
        public string? Total { get; set; }
        public string? Expire { get; set; }
        public List<CashData> Cash { get; set; }

        public CashHistory()
        {
            Cash = new List<CashData>();
        }

    }
}
