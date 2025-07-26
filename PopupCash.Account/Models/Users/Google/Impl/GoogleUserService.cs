using System.Net.Http;
using System.Net.Http.Headers;

using PopupCash.Account.Extensions;
using PopupCash.Account.Models.Helpers;
using PopupCash.Account.Models.Socials.Google;
using PopupCash.Core.Models.Constants;

namespace PopupCash.Account.Models.Users.Google.Impl
{
    internal class GoogleUserService : IGoogleUserService
    {
        private const string _apiUrlBase = "https://www.googleapis.com/oauth2";

        private readonly IHttpClientFactory _httpClientFactory;

        public GoogleUserService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> GetSocialUserEmail(string accessToken)
        {
            if (await GetUserInfoAsync(accessToken) is not GoogleUserResponse user) throw new Exception($"{ConstantString.GoogleName}에서 User 정보를 가져오는데 실패하였습니다.");
            if (string.IsNullOrEmpty(user.Email)) throw new Exception($"{ConstantString.GoogleName}에서 E-MAIL 정보를 가져오는데 실패하였습니다.");

            return user.Email ??= string.Empty;
        }

        public async Task<GoogleUserResponse> GetUserInfoAsync(string accessToken)
        {
            var url = UriHelper.CombineUri(_apiUrlBase, $"v3/userinfo");
            var client = _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            return await client.GetAsync<GoogleUserResponse>(url).ConfigureAwait(false);
        }

        /// <summary>
        /// 각 SNS Service의 경우 해당 메서드는 구현하지 않는다.
        /// </summary>
        /// <param name="name"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void SetSocialService(string name)
        {
            throw new NotImplementedException();
        }
    }
}
