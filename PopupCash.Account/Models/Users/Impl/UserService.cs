using System.Net.Http;
using PopupCash.Account.Extensions;

namespace PopupCash.Account.Models.Users.Impl
{
    public class UserService : IUserService
    {
        private const string _baseUrl = "https://api.popupcash.co.kr/api/";
        private const string _apiVersion = "v1";

        /// <summary>
        /// httpClientFactory
        /// </summary>
        private readonly IHttpClientFactory _httpClientFactory;
        public UserService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<UserResponse> GetCurrentUser(string accesstoken)
        {
            var client = _httpClientFactory.CreateClient();
            client.AddDefaultHeaders();

            // API 엔드포인트 URL 지정
            string apiUrl = $"{_baseUrl}{_apiVersion}/user";

            client.DefaultRequestHeaders.Add("x-access-token", accesstoken);

            return await client.GetAsync<UserResponse>(apiUrl).ConfigureAwait(false);
        }

    }
}
