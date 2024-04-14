namespace PopupCash.Database.Models.Users
{
    public class User
    {
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? Cash { get; set; }

        public string? Flag { get; set; }

        public int MissionPoint { get; set; }
        public string? Grade { get; set; }

    }
}
