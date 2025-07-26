using PopupCash.Account.Models.Socials.Kakaos;

namespace PopupCash.Account.Models.Users.Kakao
{
    public interface IKakaoUserService : ISocialUserService
    {
        public Task<KakaoUserResponse> GetUserInfoAsync(string accessToken);
    }
}
