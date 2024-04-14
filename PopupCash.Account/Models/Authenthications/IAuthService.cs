using PopupCash.Account.Models.Authenthications.Impl;

namespace PopupCash.Account.Models.Authenthications
{
    public interface IAuthService
    {
        public string GetAuthCodeUrl();
        public string GetRedirectUrl();

        public Task<bool> IsValidateTokenAsync(string accessToken);
        public Task<AuthTokenInfo> GetAuthorizationTokenAsync(string code);

        //public Task<TerminateResponse> TerminateTokenAsync<TerminateResponse>(string accessToken);
    }
}
