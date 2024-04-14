using PopupCash.Account.Models.Cashs.Impl;
using PopupCash.Account.Models.Users.Impl;

namespace PopupCash.Account.Models.Cashs
{
    public interface ICashService
    {
        /// <summary>
        /// 적립 내역
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public Task<CashSaveHistoryResponse> GetCashSaveHistory(string accessToken);

        #region Pomission API
        /// <summary>
        /// Pomission Access Token
        /// </summary>
        /// <returns></returns>
        public Task<PomissionAccessTokenResponse> GetAccessTokenPomissionAsync(string refreshToken);
        public Task<MissionParticipationResponse> MissionParticipationPomissionAsync(string accessToken, MissionParticipationRequest requestInfo);
        #endregion
    }
}
