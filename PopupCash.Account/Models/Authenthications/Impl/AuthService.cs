using AutoMapper;
using PopupCash.Account.Models.Authenthications.Kakao;

namespace PopupCash.Account.Models.Authenthications.Impl
{
    internal class AuthService : IAuthService
    {
        // TODO. 추후 Naver, Google 추가
        // SNS Type 선택할 수 있는 enum 추가
        #region Auth Services        
        private readonly IKakaoAuthService _kakaoAuthService;
        #endregion

        public AuthService(IMapper mapper, IKakaoAuthService kakaoAuthService)
        {
            _kakaoAuthService = kakaoAuthService;
        }
        public string GetAuthCodeUrl()
        {
            return _kakaoAuthService.GetAuthCodeUrl();
        }

        public Task<AuthTokenInfo> GetAuthorizationTokenAsync(string code)
        {
            return _kakaoAuthService.GetAuthorizationTokenAsync(code);
        }

        public string GetRedirectUrl()
        {
            return _kakaoAuthService.GetRedirectUrl();
        }

        public Task<bool> IsValidateTokenAsync(string accessToken)
        {
            return _kakaoAuthService.IsValidateTokenAsync(accessToken);
        }

        public Task<TerminateResponse> TerminateTokenAsync<TerminateResponse>(string accessToken)
        {
            throw new NotImplementedException();
        }
    }
}
