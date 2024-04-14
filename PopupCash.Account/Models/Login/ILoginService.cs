using PopupCash.Account.Models.Authenthications.Impl;
using PopupCash.Account.Models.Login.Impl;

namespace PopupCash.Account.Models.Login
{
    public interface ILoginService
    {
        /// <summary>
        /// 비회원 이용 조회
        /// 로그인 전에 Key 값을 얻기 위해 사용
        /// </summary>
        /// <returns></returns>
        public Task<InitializeResponse> InitializeAsync();

        /// <summary>
        /// 비회원 이용 조회
        /// 로그인 전에 Key 값을 얻기 위해 사용
        /// </summary>
        /// <returns></returns>
        public Task<NonJoinResponse> NonJoinAsync(NonJoinRequest requestInfo);

        /// <summary>
        /// 회원 가입 요청
        /// </summary>
        /// <returns></returns>
        public Task<JoinResponse> JoinAsync(JoinRequest requestInfo);

        /// <summary>
        /// 로그인
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public Task<LoginResponse> LoginAsync(LoginRequest requestInfo);
        /// <summary>
        /// 정책
        /// </summary>
        /// <returns></returns>
        public Task<PolicyResponse> GetPolicyAsync();
        /// <summary>
        /// 개인정보 정책
        /// </summary>
        /// <returns></returns>
        public Task<PrivacyResponse> GetPrivacyAsync();



        #region SNS Login
        public string GetAuthCodeUrl();
        public Task<AuthTokenInfo?> GetTokenAsync(string uri);
        #endregion
    }
}
