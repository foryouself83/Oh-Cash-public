using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using Newtonsoft.Json;
using PopupCash.Account.Models.Cashs;
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
        private readonly ICashService _cashService;
        private readonly IAuthorizationDataService _authorizationService;
        #endregion

        // Script 정보가 유지되어야 하므로 static 선언
        private static ResponseMissonScript? _missionScript;

        public HybridAppScript(IEventAggregator eventAggregator, IDialogService dialogService,
            ICashService cashService,
            IAuthorizationDataService authorizationService)
        {
            _eventAggregator = eventAggregator;
            _dialogService = dialogService;

            _cashService = cashService;
            _authorizationService = authorizationService;
        }

        public void js_load(string message)
        {
            // 받은 메시지 처리
            Debug.WriteLine($"Received message from JavaScript: {message}");
            _missionScript = JsonConvert.DeserializeObject<ResponseMissonScript>(message);

            _eventAggregator.GetEvent<MissionScriptEvent>().Publish(_missionScript);
        }

        public void startScrollToBottom()
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                _dialogService.ShowDialog("ActivityIndicator",
                new ActivityIndicatorDialogParameter("자동 스크롤 후 ", Brushes.Yellow, FontWeights.Normal, "적립이 됩니다. \n기다려주세요", Brushes.White, FontWeights.Normal, true),
                new Action<IDialogResult>((result) =>
                {
                    if (result.Result != ButtonResult.OK) { return; }
                }));
            }));
        }
        public async Task arrivedBottom()
        {
            // Close ActivityIndicator 
            _eventAggregator.GetEvent<ActivityIndicatorEvent>().Publish(new ActivityEventParameter() { IsClose = true });

            // close ContentBrowser
            _eventAggregator.GetEvent<CloseDialogEvent>().Publish(nameof(ContentBrowserDialog));

            try
            {
                if (_authorizationService.SelectLastestAuthorization() is not Authorization authorization) throw new Exception("토큰 정보가 없습니다.");

                if (_missionScript is null) throw new Exception("스크립트 정보가 없습니다.");

                var resultAccessToken = await _cashService.GetAccessTokenPomissionAsync("eyJhbGciOiJIUzI1NiJ9.eyJpYXQiOjE3MTMyNTczMjZ9.1T4nAy8l_WAvp9mktE-FqfCdms9VQ4Irq-CJuU4TCp8");

                if (resultAccessToken.Result == 0)
                {
                    var result = await _cashService.MissionParticipationPomissionAsync(resultAccessToken.Token!, new Account.Models.Cashs.Impl.MissionParticipationRequest(_missionScript.MissionSeq, _missionScript.MissionId, authorization.Key));

                    NLog.LogManager.GetCurrentClassLogger().Debug($"result : {result.Result} msg:{result.Msg}");
                    // Current User 정보를 업데이트
                    _eventAggregator.GetEvent<UpdateCurrentUserEvent>().Publish(true);
                }
                else
                    throw new Exception($"{resultAccessToken.Msg}");
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
        }
    }
}