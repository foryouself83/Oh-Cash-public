using PopupCash.Account.Models.Authenthications.Impl;
using PopupCash.Account.Models.Login.Impl;

namespace PopupCash.Account.Models.Authenthications
{
    public interface IAuthService
    {
        public void SetInitData(InitializeResponse response);
        public void SetSocialService(string name);
        public string GetAuthCodeUrl();
        public string GetRedirectUrl();

        /// <summary>
        /// 토큰 받기
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Task<AuthTokenInfo> GetAuthorizationTokenAsync(string code);

        /// <summary>
        /// 연결 끊기
        /// </summary>
        /// <returns></returns>
        public Task<bool> UnlinkAsync(string accessToken);

        //public Task<TerminateResponse> TerminateTokenAsync<TerminateResponse>(string accessToken);
    }
}
