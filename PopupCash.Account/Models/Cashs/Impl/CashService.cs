using System.Net.Http;
using PopupCash.Account.Extensions;
using PopupCash.Account.Models.Users.Impl;

namespace PopupCash.Account.Models.Cashs.Impl
{
    internal class CashService : ICashService
    {
        private const string _apiVersion = "v1";
        private const string _baseUrl = "https://api.popupcash.co.kr/api/";
        private const string _basePomissionUrl = "https://api.pomission.com/api/";
        /// <summary>
        /// httpClientFactory
        /// </summary>
        private readonly IHttpClientFactory _httpClientFactory;
        public CashService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<PomissionAccessTokenResponse> GetAccessTokenPomissionAsync(string refreshToken)
        {
            var client = _httpClientFactory.CreateClient();
            client.AddDefaultHeaders();

            // API 엔드포인트 URL 지정
            string apiUrl = $"{_basePomissionUrl}{_apiVersion}/common/getAccessToken?media_id=popupcashpc";

            client.DefaultRequestHeaders.Add("x-refresh-token", refreshToken);

            return await client.GetAsync<PomissionAccessTokenResponse>(apiUrl).ConfigureAwait(false);
<<<<<<< HEAD
<<<<<<< HEAD

        }

        public async Task<CashSaveHistoryResponse> GetCashSaveHistory(string accesstoken)
=======
        }

        public async Task<CashSaveHistoryResponse> GetCashSaveHistory(string accessToken)
>>>>>>> ba01ad0 (Feat: 미션 완료 적립 API 추가 진행 #1)
=======

        }

        public async Task<CashSaveHistoryResponse> GetCashSaveHistory(string accesstoken)
>>>>>>> 8b539a1 (Feat: 미션 완료 적립 API 추가)
        {
            var client = _httpClientFactory.CreateClient();
            client.AddDefaultHeaders();

            // API 엔드포인트 URL 지정
            string apiUrl = $"{_baseUrl}{_apiVersion}/cash/save";

            client.DefaultRequestHeaders.Add("x-access-token", accesstoken);

            return await client.GetAsync<CashSaveHistoryResponse>(apiUrl).ConfigureAwait(false);
        }
<<<<<<< HEAD

        public async Task<MissionParticipationResponse> MissionParticipationPomissionAsync(string accessToken, MissionParticipationRequest requestInfo)
        {
            var client = _httpClientFactory.CreateClient();
            client.AddDefaultHeaders();

            client.DefaultRequestHeaders.Add("x-access-token", accessToken);

            // API 엔드포인트 URL 지정
            string apiUrl = $"{_basePomissionUrl}{_apiVersion}/mission/auto/participation";
            string mac = MacAddressHelper.GetMacAddress();
            requestInfo.Mac = $"{mac}";

            return await client.PostAsync<MissionParticipationResponse>(apiUrl, requestInfo).ConfigureAwait(false);
        }

        public async Task<SaveCashResponse> SaveCashAsync(string accessToken, SaveCashRequest requestInfo)
        {
=======
>>>>>>> ba01ad0 (Feat: 미션 완료 적립 API 추가 진행 #1)

        public async Task<MissionParticipationResponse> MissionParticipationPomissionAsync(string accessToken, MissionParticipationRequest requestInfo)
        {
            var client = _httpClientFactory.CreateClient();
            client.AddDefaultHeaders();

            client.DefaultRequestHeaders.Add("x-access-token", accessToken);

            // API 엔드포인트 URL 지정
            string apiUrl = $"{_basePomissionUrl}{_apiVersion}/mission/auto/participation";

            return await client.PostAsync<MissionParticipationResponse>(apiUrl, requestInfo).ConfigureAwait(false);
        }

        public async Task<SaveCashResponse> SaveCashAsync(string accessToken, SaveCashRequest requestInfo)
        {

            var client = _httpClientFactory.CreateClient();
            client.AddDefaultHeaders();
            client.DefaultRequestHeaders.Add("x-access-token", accessToken);

            // API 엔드포인트 URL 지정
            string apiUrl = $"{_baseUrl}v4/cash";
            requestInfo.Type = "2";

            return await client.PostAsync<SaveCashResponse>(apiUrl, requestInfo).ConfigureAwait(false);
        }
    }
}
