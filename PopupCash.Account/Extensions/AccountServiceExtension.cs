using Microsoft.Extensions.DependencyInjection;

using PopupCash.Account.Models.Authenthications;
using PopupCash.Account.Models.Authenthications.Google;
using PopupCash.Account.Models.Authenthications.Google.Impl;
using PopupCash.Account.Models.Authenthications.Impl;
using PopupCash.Account.Models.Authenthications.Kakao;
using PopupCash.Account.Models.Authenthications.Kakao.Impl;
using PopupCash.Account.Models.Authenthications.Naver;
using PopupCash.Account.Models.Authenthications.Naver.Impl;
using PopupCash.Account.Models.Cashs;
using PopupCash.Account.Models.Cashs.Impl;
using PopupCash.Account.Models.Users;
using PopupCash.Account.Models.Users.Google;
using PopupCash.Account.Models.Users.Google.Impl;
using PopupCash.Account.Models.Users.Impl;
using PopupCash.Account.Models.Users.Kakao;
using PopupCash.Account.Models.Users.Kakao.Impl;
using PopupCash.Account.Models.Users.Naver;
using PopupCash.Account.Models.Users.Naver.Impl;

namespace PopupCash.Account.Extensions
{
    public static class AccountServiceExtension
    {
        public static void AccountService(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IKakaoAuthService, KakaoAuthService>()
                .AddSingleton<INaverAuthService, NaverAuthService>()
                .AddSingleton<IGoogleAuthService, GoogleAuthService>()
                .AddSingleton<IAuthService, AuthService>()
                .AddSingleton<ICashService, CashService>()
                .AddSingleton<IGoogleUserService, GoogleUserService>()
                .AddSingleton<INaverUserService, NaverUserService>()
                .AddSingleton<IKakaoUserService, KakaoUserService>()
                .AddSingleton<ISocialUserService, SocialUserService>();

        }
    }
}
