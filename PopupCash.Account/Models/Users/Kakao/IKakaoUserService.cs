using PopupCash.Account.Models.Users.Kakao.Impl;

namespace PopupCash.Account.Models.Users.Kakao
{
    public interface IKakaoUserService : ISocialUserService
    {
        public Task<KakaoUser> GetUserInfoAsync(string accessToken);
    }
}
