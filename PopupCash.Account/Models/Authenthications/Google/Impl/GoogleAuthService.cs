using System.Net.Http;
using System.Net.Http.Headers;

using AutoMapper;

using PopupCash.Account.Extensions;
using PopupCash.Account.Models.Authenthications.Impl;
using PopupCash.Account.Models.Helpers;
using PopupCash.Account.Models.Login.Impl;
using PopupCash.Account.Models.Socials.Google;
using PopupCash.Account.Models.Socials.Kakaos;
using PopupCash.Core.Models.Constants;

namespace PopupCash.Account.Models.Authenthications.Google.Impl
{
    public class GoogleAuthService : IGoogleAuthService
    {
        private const string _apiUrlBase = "https://oauth2.googleapis.com";
        private const string _authUrlBase = "https://accounts.google.com/o/oauth2";

        /// <summary>
        /// httpClientFactory
        /// </summary>
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMapper _mapper;

        public GoogleRestKey? GoogleRestKey { get; private set; }
        public GoogleAuthService(IHttpClientFactory httpClientFactory, IMapper mapper)
        {
            _httpClientFactory = httpClientFactory;
            _mapper = mapper;
        }
        public void SetInitData(InitializeResponse response)
        {
            if (response.GoogleRestKey is null) throw new Exception($"{ConstantString.GoogleName}에 대한 키 값이 없습니다. ");
            this.GoogleRestKey = response.GoogleRestKey;
        }
        public string GetRedirectUrl()
        {
            if (GoogleRestKey is null) throw new Exception($"{ConstantString.GoogleName}에 대한 키 값이 없습니다. ");
            return GoogleRestKey.RedirectUri!;
        }

        public string GetAuthCodeUrl()
        {
            if (GoogleRestKey is null) throw new Exception($"{ConstantString.GoogleName}에 대한 키 값이 없습니다. ");

            return UriHelper.CombineUri(_authUrlBase, $"/v2/auth?client_id={GoogleRestKey.ClientId}&redirect_uri={GoogleRestKey.RedirectUri}&response_type=code&scope=email%20profile%20openid&access_type=offline");
        }

        public async Task<bool> UnlinkAsync(string accessToken)
        {
            string data = string.Format($"token={accessToken}");
            var uri = UriHelper.CombineUri(_apiUrlBase, $"/revoke");

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var requestBody = new StringContent(data);
            requestBody.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var response = await client.PostAsync<GoogleUnlinkResponse>(uri, requestBody).ConfigureAwait(false);

            return response.Id != 0;
        }
        public async Task<AuthTokenInfo> GetAuthorizationTokenAsync(string code)
        {
            if (GoogleRestKey is null) throw new Exception($"{ConstantString.GoogleName}에 대한 키 값이 없습니다. ");

            string data = string.Format($"grant_type=authorization_code&client_id={GoogleRestKey.ClientId}&client_secret={GoogleRestKey.ClientSecret}&redirect_uri={GoogleRestKey.RedirectUri}&code={code}");
            var uri = UriHelper.CombineUri(_apiUrlBase, $"/token");

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "");

            var requestBody = new StringContent(data);
            requestBody.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var response = await client.PostAsync<GoogleTokenResponse>(uri, requestBody).ConfigureAwait(false);

            return _mapper.Map<AuthTokenInfo>(response);
        }


        public async Task<AuthTokenInfo> RefreshAuthorizationTokenAsync(string refreshToken)
        {
            if (GoogleRestKey is null) throw new Exception($"{ConstantString.GoogleName}에 대한 키 값이 없습니다. ");

            string data = string.Format($"grant_type=refresh_tokene&client_id={GoogleRestKey.ClientId}&client_secret={GoogleRestKey.ClientSecret}&refresh_token={refreshToken}");
            var uri = UriHelper.CombineUri(_apiUrlBase, $"/token");

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "");

            var requestBody = new StringContent(data);
            requestBody.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var response = await client.PostAsync<GoogleRefreshTokenResponse>(uri, requestBody).ConfigureAwait(false);

            return _mapper.Map<AuthTokenInfo>(response);
        }

        /// <summary>
        /// Terminate Authentication
        /// </summary>
        /// <returns></returns>
        public async Task<KakaoLogout> TerminateTokenAsync(string accessToken)
        {
            var uri = UriHelper.CombineUri(_authUrlBase, $"/v1/user/logout");

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var requestBody = new StringContent("");
            requestBody.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            return await client.PostAsync<KakaoLogout>(uri, requestBody).ConfigureAwait(false);
        }

        public void SetSocialService(string name)
        {
            throw new NotImplementedException();
        }
    }
}
