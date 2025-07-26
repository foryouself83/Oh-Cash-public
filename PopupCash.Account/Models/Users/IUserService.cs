using PopupCash.Account.Models.Users.Impl;

namespace PopupCash.Account.Models.Users
{
    public interface IUserService
    {
        /// <summary>
        /// 유저 정보
        /// </summary>
        /// <param name="accesstoken"></param>
        /// <returns></returns>
        public Task<UserResponse> GetCurrentUser(string accesstoken);

        /// <summary>
        /// SNS 로그인할 서비스 선택
        /// </summary>
        /// <param name="name"></param>
        public void SetSocialService(string name);

        /// <summary>
        /// SNS 로그인 후 Email 주소를 알아온다.
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public Task<string> GetSocialUserEmail(string accessToken);
    }
}
