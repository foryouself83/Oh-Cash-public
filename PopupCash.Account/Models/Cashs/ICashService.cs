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
<<<<<<< HEAD
<<<<<<< HEAD
        public Task<MissionParticipationResponse> MissionParticipationPomissionAsync(string accessToken, MissionParticipationRequest requestInfo);
=======
        public Task<PomissionAccessTokenResponse> MissionParticipationPomissionAsync(string accessToken, MissionParticipationRequest requestInfo);
>>>>>>> ba01ad0 (Feat: 미션 완료 적립 API 추가 진행 #1)
=======
        public Task<MissionParticipationResponse> MissionParticipationPomissionAsync(string accessToken, MissionParticipationRequest requestInfo);
>>>>>>> 8b539a1 (Feat: 미션 완료 적립 API 추가)
        #endregion
    }
}
