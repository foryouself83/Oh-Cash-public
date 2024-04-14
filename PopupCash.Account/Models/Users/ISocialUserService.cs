namespace PopupCash.Account.Models.Users
{
    public interface ISocialUserService
    {
        public void SetSocialService(string name);

        public Task<string> GetSocialUserEmail(string accessToken);

    }
}
