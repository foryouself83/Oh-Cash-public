using PopupCash.Account.Models.Users.Google;
using PopupCash.Account.Models.Users.Kakao;
using PopupCash.Account.Models.Users.Naver;
using PopupCash.Core.Models.Constants;

namespace PopupCash.Account.Models.Users.Impl
{
    internal class SocialUserService : ISocialUserService
    {
        private ISocialUserService? _socialUserService;

        private readonly IKakaoUserService _kakaoUserService;
        private readonly INaverUserService _naverUserService;
        private readonly IGoogleUserService _googleUserService;

        public SocialUserService(IKakaoUserService kakaoUserService, INaverUserService naverUserService, IGoogleUserService googleUserService)
        {
            _kakaoUserService = kakaoUserService;
            _naverUserService = naverUserService;
            _googleUserService = googleUserService;
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
                case ConstantString.NaverName:
                    {
                        _socialUserService = _naverUserService;
                    }
                    break;
                case ConstantString.GoogleName:
                    {
                        _socialUserService = _googleUserService;
                    }
                    break;
            }
        }
    }
}
