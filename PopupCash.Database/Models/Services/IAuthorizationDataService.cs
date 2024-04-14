using PopupCash.Database.Models.Users;

namespace PopupCash.Database.Models.Services
{
    public interface IAuthorizationDataService
    {
        /// <summary>
        /// Create Authorization Table
        /// </summary>
        /// <returns></returns>
        public bool CreateTable();

        /// <summary>
        /// 가장 마지막에 로그인한 Authorization를 가져온다
        /// </summary>
        /// <returns></returns>
        public Authorization? SelectLastestAuthorization();
        /// <summary>
        /// 마지막으로 로그인한 Access token 을 Limit 만큼 가져온다.
        /// </summary>
        /// <param name="limit"></param>
        /// <returns></returns>
        public IEnumerable<string> SelectOrderByAccesToken(int limit);

        /// <summary>
        /// Insert Authorization
        /// </summary>
        /// <param name="authorization"></param>
        /// <returns></returns>
        public bool InsertAuthorization(Authorization authorization);

        /// <summary>
        /// Update Authorization
        /// </summary>
        /// <param name="authorization"></param>
        /// <returns></returns>
        public bool UpdateAuthorization(Authorization authorization);

        /// <summary>
        /// Delete Access Token
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteAuthorization(string id);
    }
}
