using System.Net.Http;
using System.Windows;

using AutoMapper;

using PopupCash.Account.Extensions;
using PopupCash.Account.Models.Authenthications;
using PopupCash.Account.Models.Authenthications.Impl;
using PopupCash.Account.Models.Helpers;

namespace PopupCash.Account.Models.Login.Impl
{
    public class LoginService : ILoginService
    {
        private const string _apiVersion = "v1";
        private const string _baseUrl = "https://api.popupcash.co.kr/api/";

        private readonly IMapper _mapper;

        /// <summary>
        /// httpClientFactory
        /// </summary>
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IAuthService _authService;

        public PomissionInfo? PomissionInfo { get; private set; }

        public LoginService(IMapper mapper, IHttpClientFactory httpClientFactory, IAuthService authService)
        {
            _mapper = mapper;
            _httpClientFactory = httpClientFactory;
            _authService = authService;

        }
        public async Task<NonJoinResponse> NonJoinAsync(NonJoinRequest requestInfo)
        {
            var client = _httpClientFactory.CreateClient();
            client.AddDefaultHeaders();

            // API 엔드포인트 URL 지정
            string apiUrl = $"{_baseUrl}{_apiVersion}/non-join";
            string mac = MacAddressHelper.GetMacAddress();
            requestInfo.Mac = $"{mac}";
            requestInfo.AdId = $"{mac}";
            //requestInfo.adid = "{32d30c1d-45b6-469a-ac6d}";
            requestInfo.Non = "N"; // 비회원 이용 구분

            return await client.PostAsync<NonJoinResponse>(apiUrl, requestInfo).ConfigureAwait(false);
        }
        public async Task<JoinResponse> JoinAsync(JoinRequest requestInfo)
        {
            var client = _httpClientFactory.CreateClient();
            client.AddDefaultHeaders();

            // API 엔드포인트 URL 지정
            string apiUrl = $"{_baseUrl}{_apiVersion}/join";
            string mac = MacAddressHelper.GetMacAddress();
            requestInfo.AdId = $"{mac}";
            requestInfo.Mac = $"{mac}";
            requestInfo.DeviceId = $"{mac}";
            requestInfo.DeviceModel = "PC";
            requestInfo.DeviceOs = Environment.OSVersion.VersionString;
            requestInfo.AppVersion = Application.Current.GetType().Assembly.GetName().Version?.ToString();


            return await client.PostAsync<JoinResponse>(apiUrl, requestInfo).ConfigureAwait(false);
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest requestInfo)
        {
            var client = _httpClientFactory.CreateClient();
            client.AddDefaultHeaders();

            // API 엔드포인트 URL 지정
            string apiUrl = $"{_baseUrl}{_apiVersion}/login";
            string mac = MacAddressHelper.GetMacAddress();
            requestInfo.Mac = $"{mac}";
            requestInfo.AdId = $"{mac}";

            return await client.PostAsync<LoginResponse>(apiUrl, requestInfo).ConfigureAwait(false);
        }

        public async Task<PolicyResponse> GetPolicyAsync()
        {
            var client = _httpClientFactory.CreateClient();
            client.AddDefaultHeaders();

            // API 엔드포인트 URL 지정
            string apiUrl = $"{_baseUrl}{_apiVersion}/policy/service";

            return await client.GetAsync<PolicyResponse>(apiUrl).ConfigureAwait(false);
        }

        public async Task<PrivacyResponse> GetPrivacyAsync()
        {
            var client = _httpClientFactory.CreateClient();
            client.AddDefaultHeaders();

            // API 엔드포인트 URL 지정
            string apiUrl = $"{_baseUrl}{_apiVersion}/policy/privacy";

            return await client.GetAsync<PrivacyResponse>(apiUrl).ConfigureAwait(false);
        }

        public async Task<InitializeResponse> InitializeAsync()
        {
            var client = _httpClientFactory.CreateClient();
            client.AddDefaultHeaders();

            // API 엔드포인트 URL 지정
            string apiUrl = $"{_baseUrl}{_apiVersion}/init";

            return await client.GetAsync<InitializeResponse>(apiUrl).ConfigureAwait(false);
        }

        #region Auth Services
        public void SetSocialService(string name)
        {
            _authService.SetSocialService(name);
        }
        public string GetAuthCodeUrl()
        {
            return _authService.GetAuthCodeUrl();
        }

        public async Task<AuthTokenInfo?> GetTokenAsync(string uri)
        {
            var unescapedUrl = System.Net.WebUtility.UrlDecode(uri);
            if (string.IsNullOrEmpty(unescapedUrl)) return null;

            // Redirect URI로 전달해줘야 성공
            if (!unescapedUrl.StartsWith(_authService.GetRedirectUrl())) return null;

            var authResponse = new IdentityModel.Client.AuthorizeResponse(unescapedUrl);
            if (string.IsNullOrEmpty(authResponse.Code)) return null;

            return await _authService.GetAuthorizationTokenAsync(authResponse.Code);
        }

        public async Task<bool> UnlinkAsync(string accessToken)
        {
            return await _authService.UnlinkAsync(accessToken);
        }

        public void SetInitData(InitializeResponse response)
        {
            if (response.PomissionInfo is null) throw new Exception($"Pomission 에 대한 키 값이 없습니다. ");
            this.PomissionInfo = response.PomissionInfo;
            _authService.SetInitData(response);
        }

        public PomissionInfo? GetPomissionInfo()
        {
            return PomissionInfo;
        }

        #endregion
    }
}
