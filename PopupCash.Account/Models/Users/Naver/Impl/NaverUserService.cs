using System.Net.Http;
using System.Net.Http.Headers;

using PopupCash.Account.Extensions;
using PopupCash.Account.Models.Helpers;
using PopupCash.Account.Models.Socials.Navers;
using PopupCash.Core.Models.Constants;

using static PopupCash.Account.Models.Socials.Navers.NaverUserResponse;

namespace PopupCash.Account.Models.Users.Naver.Impl
{
    internal class NaverUserService : INaverUserService
    {
        private const string _apiUrlBase = "https://openapi.naver.com";

        private readonly IHttpClientFactory _httpClientFactory;

        public NaverUserService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> GetSocialUserEmail(string accessToken)
        {
            if (await GetUserInfoAsync(accessToken) is not NaverUserResponse user) throw new Exception($"{ConstantString.NaverName}에서 User 정보를 가져오는데 실패하였습니다.");
            if (user.Response is not ResponseData ResponseData) throw new Exception($"{ConstantString.NaverName}에서 Account 정보를 가져오는데 실패하였습니다.");
            if (string.IsNullOrEmpty(ResponseData.Email)) throw new Exception($"{ConstantString.NaverName}에서 E-MAIL 정보를 가져오는데 실패하였습니다.");

            return ResponseData.Email ??= string.Empty;
        }

        public async Task<NaverUserResponse> GetUserInfoAsync(string accessToken)
        {
            var url = UriHelper.CombineUri(_apiUrlBase, $"/v1/nid/me");
            var client = _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            return await client.GetAsync<NaverUserResponse>(url).ConfigureAwait(false);
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
