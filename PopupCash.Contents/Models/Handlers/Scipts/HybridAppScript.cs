using System.Diagnostics;
using System.Windows;
using System.Windows.Media;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using PopupCash.Account.Models.Cashs;
using PopupCash.Account.Models.Login;
using PopupCash.Account.Models.Login.Impl;
using PopupCash.Common.Models.Dialogs.Parameters;
using PopupCash.Common.Models.Events;
using PopupCash.Contents.Models.Events;
using PopupCash.Contents.Views;
using PopupCash.Core.Models.Constants;
using PopupCash.Database.Models.Services;
using PopupCash.Database.Models.Users;

using Prism.Events;
using Prism.Services.Dialogs;

namespace PopupCash.Contents.Models.Handlers.Scipts
{
    public class HybridAppScript
    {
        #region Prism
        private readonly IEventAggregator _eventAggregator;
        private readonly IDialogService _dialogService;
        #endregion

        #region Services
        private readonly ILoginService _loginService;
        private readonly ICashService _cashService;
        private readonly IAuthorizationDataService _authorizationService;
        private readonly IUserDataService _userDataService;
        ILogger _logger;
        #endregion

        // Script 정보가 유지되어야 하므로 static 선언
        private static ResponseMissonScript? _missionScript;

        public HybridAppScript(IEventAggregator eventAggregator, IDialogService dialogService,
            ILoginService loginService, ICashService cashService,
            IAuthorizationDataService authorizationService, IUserDataService userDataService,
            ILogger<HybridAppScript> loggor)
        {
            _eventAggregator = eventAggregator;
            _dialogService = dialogService;

            _loginService = loginService;
            _cashService = cashService;
            _authorizationService = authorizationService;
            _userDataService = userDataService;

            _logger = loggor;
        }

        public void js_load(string message)
        {
            _logger.LogDebug($"스크립트 불러오기 성공");

            // 받은 메시지 처리
            Debug.WriteLine($"Received message from JavaScript: {message}");

            _eventAggregator.GetEvent<MissionScriptEvent>().Publish(message);
            _missionScript = JsonConvert.DeserializeObject<ResponseMissonScript>(message);
        }

        public void startScrollToBottom()
        {
            _logger.LogDebug($"스크립트 스크롤 동작 시작");

            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                _dialogService.ShowDialog("ActivityIndicator",
                new ActivityIndicatorDialogParameter("ContentBrowser", "자동 스크롤 후 ", Brushes.Yellow, FontWeights.Normal, "적립이 됩니다. \n기다려주세요", Brushes.White, FontWeights.Normal, true),
                new Action<IDialogResult>((result) =>
                {
                    if (result.Result != ButtonResult.OK) { return; }
                }));
            }), System.Windows.Threading.DispatcherPriority.Send);
        }
        public async Task arrivedBottom()
        {
            _logger.LogDebug($"스크립트 동작 완료");
            // Close ActivityIndicator 
            _eventAggregator.GetEvent<ActivityIndicatorEvent>().Publish(new ActivityEventParameter() { IsClose = true });


            try
            {
                if (_authorizationService.SelectLastestAuthorization() is not Authorization authorization) throw new Exception("토큰 정보가 없습니다.");
                if (_userDataService.SelectUser(authorization.AccessToken) is not UserData userData) throw new Exception("사용자 정보가 없습니다.");

                if (_missionScript is null) throw new Exception("스크립트 정보가 없습니다.");

                if (_loginService.GetPomissionInfo() is not PomissionInfo pomissionInfo) throw new Exception("Pomission 정보가 없습니다.");
                if (string.IsNullOrEmpty(pomissionInfo.RefreshToken)) throw new Exception("Pomission token 정보가 없습니다.");

                var resultAccessToken = await _cashService.GetAccessTokenPomissionAsync(pomissionInfo.RefreshToken);

                if (resultAccessToken.Result == 0)
                {
                    var result = await _cashService.MissionParticipationPomissionAsync(resultAccessToken.Token!, new Account.Models.Cashs.Impl.MissionParticipationRequest(_missionScript.MissionSeq, _missionScript.MissionId, authorization.PomissionKey, userData.MacAddress));

                    _logger.LogDebug($"result : {result.Result} msg:{result.Msg}");
                    await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        _dialogService.ShowDialog("InformationDialog",
                        new InformationDialogParameter($"{_missionScript.UserPoint}P", ConstantColors.DefaultTextPointColor, FontWeights.Normal,
                        "적립이 완료되었습니다. ", ConstantColors.DefaultMessageColor, FontWeights.Normal, "3초 후 자동으로 종료됩니다.", true, false, 3, "ContentBrowser"),
                          new Action<IDialogResult>((result) =>
                          {
                              if (result.Result != ButtonResult.OK) { return; }
                          }));
                    }));

                    _eventAggregator.GetEvent<RefreshMissionBrowserEvent>().Publish(new Core.Models.Parameters.MoveAddressParameter());
                    // Current User 정보를 업데이트
                    _eventAggregator.GetEvent<UpdateCurrentUserEvent>().Publish(true);

                    if (result.Result != 0)
                    {
                        throw new Exception($"{result.Msg}");
                    }
                }
                else
                {
                    _logger.LogDebug($"Result: {resultAccessToken.Result}");
                    throw new Exception($"{resultAccessToken.Msg}");
                }
            }
            catch (Exception e)
            {
                await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {

                    _dialogService.ShowDialog("InformationDialog",
                   new InformationDialogParameter(e.Message, ConstantColors.DefaultMessageColor, FontWeights.Normal, string.Empty, ConstantColors.DefaultMessageColor, FontWeights.Normal, "확인", false, false),
                   new Action<IDialogResult>((result) =>
                   {
                       if (result.Result != ButtonResult.OK) { return; }
                   }));
                }));
            }
            finally
            {
                // close ContentBrowser
                _eventAggregator.GetEvent<CloseDialogEvent>().Publish(nameof(ContentBrowserDialog));
            }
        }
    }
}