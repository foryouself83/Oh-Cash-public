using System.Net.Http;
using System.Net.Http.Headers;
using AutoMapper;
using PopupCash.Account.Extensions;
using PopupCash.Account.Models.Authenthications.Impl;
using PopupCash.Account.Models.Helpers;
using PopupCash.Account.Models.Login.Impl;
using PopupCash.Account.Models.Socials.Kakaos;
using PopupCash.Core.Models.Constants;

namespace PopupCash.Account.Models.Authenthications.Kakao.Impl
{
    internal class KakaoAuthService : IKakaoAuthService
    {
        private const string _apiUrlBase = "https://kapi.kakao.com";
        private const string _authUrlBase = "https://kauth.kakao.com/oauth";

        /// <summary>
        /// httpClientFactory
        /// </summary>
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMapper _mapper;

        public KakaoRestKey? KakaoRestKey { get; private set; }

        public KakaoAuthService(IHttpClientFactory httpClientFactory, IMapper mapper)
        {
            _httpClientFactory = httpClientFactory;
            _mapper = mapper;
        }


        public void SetInitData(InitializeResponse response)
        {
            if (response.KakaoRestKey is null) throw new Exception($"{ConstantString.KakaoName}에 대한 키 값이 없습니다. ");
            this.KakaoRestKey = response.KakaoRestKey;
        }
        public string GetRedirectUrl()
        {
            if (KakaoRestKey is null) throw new Exception($"{ConstantString.KakaoName}에 대한 키 값이 없습니다. ");

            return KakaoRestKey.RedirectUri!;
        }

        public string GetAuthCodeUrl()
        {
            if (KakaoRestKey is null) throw new Exception($"{ConstantString.KakaoName}에 대한 키 값이 없습니다. ");

            return UriHelper.CombineUri(_authUrlBase, $"/authorize?response_type=code&client_id={KakaoRestKey.RestApiKey}&redirect_uri={KakaoRestKey.RedirectUri}");
        }

        public async Task<bool> UnlinkAsync(string accessToken)
        {
            var uri = UriHelper.CombineUri(_apiUrlBase, $"/v1/user/unlink");

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.GetAsync<KakaoUnlinkResponse>(uri).ConfigureAwait(false);

            return response.Id != 0;
        }
        public async Task<AuthTokenInfo> GetAuthorizationTokenAsync(string code)
        {
            if (KakaoRestKey is null) throw new Exception($"{ConstantString.KakaoName}에 대한 키 값이 없습니다. ");
            string data = string.Format($"grant_type=authorization_code&client_id={KakaoRestKey.RestApiKey}&redirect_uri={KakaoRestKey.RedirectUri}&code={code}");
            //string data = string.Format($"grant_type=authorization_code&client_id={KakaoRestKey.RestApiKey}&redirect_uri={KakaoRestKey.RedirectUri}&code={code}&client_secret={KakaoRestKey.ClientSecret}");
            var uri = UriHelper.CombineUri(_authUrlBase, $"/token");

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "");

            var requestBody = new StringContent(data);
            requestBody.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var response = await client.PostAsync<KakaoTokenResponse>(uri, requestBody).ConfigureAwait(false);

            return _mapper.Map<AuthTokenInfo>(response);
        }

        public async Task<AuthTokenInfo> RefreshAuthorizationTokenAsync(string refreshToken)
        {
            if (KakaoRestKey is null) throw new Exception($"{ConstantString.KakaoName}에 대한 키 값이 없습니다. ");
            string data = string.Format($"grant_type=refresh_tokene&client_id={KakaoRestKey.RestApiKey}&refresh_token={refreshToken}");
            var uri = UriHelper.CombineUri(_authUrlBase, $"/token");

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "");

            var requestBody = new StringContent(data);
            requestBody.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var response = await client.PostAsync<KakaoTokenResponse>(uri, requestBody).ConfigureAwait(false);

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
