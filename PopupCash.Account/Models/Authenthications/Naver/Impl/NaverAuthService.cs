using System.Net.Http;
using System.Net.Http.Headers;

using AutoMapper;

using PopupCash.Account.Extensions;
using PopupCash.Account.Models.Authenthications.Impl;
using PopupCash.Account.Models.Helpers;
using PopupCash.Account.Models.Login.Impl;
using PopupCash.Account.Models.Socials.Navers;
using PopupCash.Core.Models.Constants;

namespace PopupCash.Account.Models.Authenthications.Naver.Impl
{
    internal class NaverAuthService : INaverAuthService
    {
        private const string _authUrlBase = "https://nid.naver.com/oauth2.0";

        /// <summary>
        /// httpClientFactory
        /// </summary>
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMapper _mapper;

        public NaverRestKey? NaverRestKey { get; private set; }

        public NaverAuthService(IHttpClientFactory httpClientFactory, IMapper mapper)
        {
            _httpClientFactory = httpClientFactory;
            _mapper = mapper;
        }
        public void SetInitData(InitializeResponse response)
        {
            if (response.NaverRestKey is null) throw new Exception($"{ConstantString.NaverName}에 대한 키 값이 없습니다. ");
            this.NaverRestKey = response.NaverRestKey;
        }
        public string GetRedirectUrl()
        {
            if (NaverRestKey is null) throw new Exception($"{ConstantString.NaverName}에 대한 키 값이 없습니다. ");
            return NaverRestKey.RedirectUri!;
        }

        public string GetAuthCodeUrl()
        {
            if (NaverRestKey is null) throw new Exception($"{ConstantString.NaverName}에 대한 키 값이 없습니다. ");

            return UriHelper.CombineUri(_authUrlBase, $"/authorize?response_type=code&client_id={NaverRestKey.ClientId}&redirect_uri={NaverRestKey.RedirectUri}&state={NaverRestKey.State}");
        }

        public async Task<bool> UnlinkAsync(string accessToken)
        {
            if (NaverRestKey is null) throw new Exception($"{ConstantString.NaverName}에 대한 키 값이 없습니다. ");
            var uri = UriHelper.CombineUri(_authUrlBase, $"/token?grant_type=delete&client_id={NaverRestKey.ClientId}&client_secret={NaverRestKey.ClientSecret}&access_token={accessToken}&service_provider=NAVER");

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("X-Naver-Client-Id", NaverRestKey.ClientId);
            client.DefaultRequestHeaders.Add("X-Naver-Client-Secret", NaverRestKey.ClientSecret);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.GetAsync<NaverUnlinkResponse>(uri).ConfigureAwait(false);

            return response.Result == "success";
        }
        public async Task<AuthTokenInfo> GetAuthorizationTokenAsync(string code)
        {
            if (NaverRestKey is null) throw new Exception($"{ConstantString.NaverName}에 대한 키 값이 없습니다. ");
            var uri = UriHelper.CombineUri(_authUrlBase, $"/token?grant_type=authorization_code&client_id={NaverRestKey.ClientId}&client_secret={NaverRestKey.ClientSecret}&redirect_uri={NaverRestKey.RedirectUri}&code={code}&state={NaverRestKey.State}");

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "");

            client.DefaultRequestHeaders.Add("X-Naver-Client-Id", NaverRestKey.ClientId);
            client.DefaultRequestHeaders.Add("X-Naver-Client-Secret", NaverRestKey.ClientSecret);


            var response = await client.GetAsync<NaverTokenResponse>(uri).ConfigureAwait(false);

            return _mapper.Map<AuthTokenInfo>(response);
        }

        public async Task<AuthTokenInfo> RefreshAuthorizationTokenAsync(string refreshToken)
        {
            if (NaverRestKey is null) throw new Exception($"{ConstantString.NaverName}에 대한 키 값이 없습니다. ");
            string data = string.Format($"grant_type=refresh_token&client_id={NaverRestKey.ClientId}&refresh_token={refreshToken}");
            var uri = UriHelper.CombineUri(_authUrlBase, $"/token");

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "");

            client.DefaultRequestHeaders.Add("X-Naver-Client-Id", NaverRestKey.ClientId);
            client.DefaultRequestHeaders.Add("X-Naver-Client-Secret", NaverRestKey.ClientSecret);

            var requestBody = new StringContent(data);
            requestBody.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var response = await client.PostAsync<NaverRefreshTokenResponse>(uri, requestBody).ConfigureAwait(false);

            return _mapper.Map<AuthTokenInfo>(response);
        }

        public void SetSocialService(string name)
        {
            throw new NotImplementedException();
        }
    }
}
