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
    }
}
