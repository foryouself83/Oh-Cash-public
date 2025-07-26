namespace PopupCash.Database.Models.Users
{
    public class UserData
    {
        public string AccessToken { get; set; }
        public string MacAddress { get; set; }
        public DateTime? UpdateDate { get; set; }

        public UserData()
        {
            AccessToken = string.Empty;
            MacAddress = string.Empty;
        }
        public UserData(string accessToken, string macAddress)
        {
            AccessToken = accessToken;
            MacAddress = macAddress;
        }
    }
}
