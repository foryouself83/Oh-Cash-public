using AutoMapper;
using PopupCash.Account.Models.Authenthications.Google;
using PopupCash.Account.Models.Authenthications.Kakao;
using PopupCash.Account.Models.Authenthications.Naver;
using PopupCash.Account.Models.Login.Impl;
using PopupCash.Core.Models.Constants;

namespace PopupCash.Account.Models.Authenthications.Impl
{
    internal class AuthService : IAuthService
    {
        private readonly IMapper _mapper;

        #region Auth Services        
        private IAuthService? _socialAuthService;

        private readonly IKakaoAuthService _kakaoAuthService;
        private readonly INaverAuthService _naverAuthService;
        private readonly IGoogleAuthService _googleAuthService;
        #endregion

        public AuthService(IMapper mapper, IKakaoAuthService kakaoAuthService, INaverAuthService naverAuthService, IGoogleAuthService googleAuthService)
        {
            _mapper = mapper;
            _kakaoAuthService = kakaoAuthService;
            _naverAuthService = naverAuthService;
            _googleAuthService = googleAuthService;
        }

        public void SetInitData(InitializeResponse response)
        {
            _kakaoAuthService.SetInitData(response);
            _naverAuthService.SetInitData(response);
            _googleAuthService.SetInitData(response);
        }
        public void SetSocialService(string name)
        {
            switch (name)
            {
                case ConstantString.KakaoName:
                    {
                        _socialAuthService = _kakaoAuthService;
                    }
                    break;
                case ConstantString.NaverName:
                    {
                        _socialAuthService = _naverAuthService;
                    }
                    break;
                case ConstantString.GoogleName:
                    {
                        _socialAuthService = _googleAuthService;
                    }
                    break;
            }
        }
        public string GetAuthCodeUrl()
        {
            if (_socialAuthService is null) throw new Exception("Social Service 가 선택되지 않았습니다.");

            return _socialAuthService.GetAuthCodeUrl();
        }

        public async Task<AuthTokenInfo> GetAuthorizationTokenAsync(string code)
        {
            if (_socialAuthService is null) throw new Exception("Social Service 가 선택되지 않았습니다.");

            return await _socialAuthService.GetAuthorizationTokenAsync(code);
        }

        public string GetRedirectUrl()
        {
            if (_socialAuthService is null) throw new Exception("Social Service 가 선택되지 않았습니다.");

            return _socialAuthService.GetRedirectUrl();
        }

        public async Task<bool> UnlinkAsync(string accessToken)
        {
            if (_socialAuthService is null) throw new Exception("Social Service 가 선택되지 않았습니다.");

            return await _socialAuthService.UnlinkAsync(accessToken);
        }
    }
}
