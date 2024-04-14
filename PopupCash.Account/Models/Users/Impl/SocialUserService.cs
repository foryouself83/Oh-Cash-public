using PopupCash.Account.Models.Users.Kakao;
using PopupCash.Core.Models.Constants;

namespace PopupCash.Account.Models.Users.Impl
{
    internal class SocialUserService : ISocialUserService
    {
        private ISocialUserService? _socialUserService;

        private readonly IKakaoUserService _kakaoUserService;

        public SocialUserService(IKakaoUserService kakaoUserService)
        {
            _kakaoUserService = kakaoUserService;
        }
        public Task<string> GetSocialUserEmail(string accessToken)
        {
            if (_socialUserService is null) throw new Exception("Social Service 가 선택되지 않았습니다.");

            return _socialUserService.GetSocialUserEmail(accessToken);
        }

        public void SetSocialService(string name)
        {
            switch (name)
            {
                case ConstantString.KakaoName:
                    {
                        _socialUserService = _kakaoUserService;
                    }
                    break;
            }
        }
    }
}
