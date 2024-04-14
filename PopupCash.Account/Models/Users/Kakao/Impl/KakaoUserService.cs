using System.Net.Http;
using System.Net.Http.Headers;
using PopupCash.Account.Extensions;
using PopupCash.Account.Models.Helpers;

namespace PopupCash.Account.Models.Users.Kakao.Impl
{
    internal class KakaoUserService : IKakaoUserService
    {
        private const string _apiUrlBase = "https://kapi.kakao.com";

        private readonly IHttpClientFactory _httpClientFactory;

        public KakaoUserService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> GetSocialUserEmail(string accessToken)
        {
            if (await GetUserInfoAsync(accessToken) is not KakaoUser user) throw new Exception("KAKAO 에서 User 정보를 가져오는데 실패하였습니다.");
            if (user.KakaoAccount is not KakaoAccount account) throw new Exception("KAKAO 에서 Account 정보를 가져오는데 실패하였습니다.");
            if (string.IsNullOrEmpty(account.Email)) throw new Exception("KAKAO 에서 E-MAIL 정보를 가져오는데 실패하였습니다.");

            return account.Email;
        }

        public async Task<KakaoUser> GetUserInfoAsync(string accessToken)
        {
            var url = UriHelper.CombineUri(_apiUrlBase, $"/v2/user/me");
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{accessToken}");
            client.DefaultRequestHeaders.Add("Content-type", "application/x-www-form-urlencoded;charset=utf-8");

            return await client.GetAsync<KakaoUser>(_apiUrlBase).ConfigureAwait(false);
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
