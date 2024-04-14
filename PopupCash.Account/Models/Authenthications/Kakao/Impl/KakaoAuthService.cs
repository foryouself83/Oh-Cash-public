using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using AutoMapper;
using PopupCash.Account.Extensions;
using PopupCash.Account.Models.Authenthications.Exceptions;
using PopupCash.Account.Models.Authenthications.Impl;
using PopupCash.Account.Models.Helpers;

namespace PopupCash.Account.Models.Authenthications.Kakao.Impl
{
    internal class KakaoAuthService : IKakaoAuthService
    {
        private const string _restKey = "52bdfe88223fe2d87baa0780f5357b56";
        private const string _redirectUri = "http://localhost:30000";
        private const string _authUrlBase = "https://kauth.kakao.com/oauth";

        /// <summary>
        /// httpClientFactory
        /// </summary>
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMapper _mapper;

        public KakaoAuthService(IHttpClientFactory httpClientFactory, IMapper mapper)
        {
            _httpClientFactory = httpClientFactory;
            _mapper = mapper;
        }
        public string GetRedirectUrl()
        {
            return _redirectUri;
        }

        public string GetAuthCodeUrl()
        {
            return UriHelper.CombineUri(_authUrlBase, $"/authorize?response_type=code&client_id={_restKey}&redirect_uri={_redirectUri}");
        }
        public async Task<AuthTokenInfo> GetAuthorizationTokenAsync(string code)
        {
            string data = string.Format($"grant_type=authorization_code&client_id={_restKey}&redirect_uri={_redirectUri}&code={code}");
            var uri = UriHelper.CombineUri(_authUrlBase, $"/token");

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "");

            var requestBody = new StringContent(data);
            requestBody.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var response = await client.PostAsync<KakaoTokenResponse>(uri, requestBody).ConfigureAwait(false);

            return _mapper.Map<AuthTokenInfo>(response);
        }

        public async Task<AuthTokenInfo> RefreshAuthorizationTokenAsync(string refreshToken, string clientSecret)
        {
            string data = string.Format($"grant_type=authorization_code&client_id={_restKey}&refresh_token={refreshToken}&client_secret={clientSecret}");
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

        public async Task<bool> IsValidateTokenAsync(string accessToken)
        {
            var uri = UriHelper.CombineUri(_authUrlBase, $"/v1/user/access_token_info");

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "");

            try
            {
                var response = await client.GetAsync<KakaoValidateTokenResponsse>(uri).ConfigureAwait(false);
            }
            catch (ServiceAuthenticationException serviceAuthenticationException)
            {
                Debug.WriteLine(serviceAuthenticationException.Content.ToString());
                return false;
            }
            catch
            {
                throw;
            }

            return true;
        }
    }
}
