using System.Windows;
using CefSharp;
using CefSharp.Handler;
using Microsoft.Extensions.Logging;
using PopupCash.Contents.Models.Events;
using PopupCash.Core.Models.Parameters;
using Prism.Events;
using Prism.Services.Dialogs;

namespace PopupCash.Contents.Models.Handlers.CefSharps
{
    /// <summary>
    /// 브라우저 헤더에 User-Agent 값 추가하고 Browser가 열리기 전에 스크립트를 전송 및 처리
    /// </summary>
    public class UserAgentWithScriptRequestHandler : RequestHandler
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IDialogService _dialogService;

        private readonly ILogger _logger;


        private readonly string readScript;

        public string? CurrentUrl;
        private string? _missonScript;
        public UserAgentWithScriptRequestHandler(IEventAggregator eventAggregator, IDialogService dialogService,
            ILogger<UserAgentWithScriptRequestHandler> loggor)
        {
            _eventAggregator = eventAggregator;
            _dialogService = dialogService;
            _logger = loggor;

            readScript = "fetch('https://pomission.com/attach/script/tracker.js').then(response => response.text()).then(data => { return data; });";

            _eventAggregator.GetEvent<MissionScriptEvent>().Subscribe(OnReceiveMissionScript);
        }
        private void OnReceiveMissionScript(string script)
        {
            _missonScript = script;
        }
        protected override bool OnBeforeBrowse(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool userGesture, bool isRedirect)
        {
            if (!request.Url.Contains("https://pomission.com/common/view/missionList"))
            {
                HandleBeforeBrowseAsync(chromiumWebBrowser, browser, request);
                return true; // 페이지 로드를 차단
            }
            return false; // 페이지 로드를 허용
        }
        private async void HandleBeforeBrowseAsync(IWebBrowser chromiumWebBrowser, IBrowser browser, IRequest request)
        {

            string url = request.Url;

            while (!chromiumWebBrowser.CanExecuteJavascriptInMainFrame)
            {
                await Task.Delay(1);
            }

            var response = await chromiumWebBrowser.EvaluateScriptAsync(readScript);
            if (response is JavascriptResponse scriptResult && scriptResult.Result != null)
            {
                var trackerScript = scriptResult.Result.ToString();
                // 읽어들인 스크립트 내용에서 jsMission 변수를 변경하는 스크립트 작성

                string script = $"var jsMission = {_missonScript}";
                string? modifyScript = trackerScript!.Replace("var jsMission = {jsMission}", script);

                await Application.Current.Dispatcher.BeginInvoke(() =>
                {
                    _dialogService.Show("ContentBrowserDialog", new MoveAddressParameter() { Url = url, TrackerScript = modifyScript },
                        new Action<IDialogResult>((result) => { }));
                });

                //_eventAggregator.GetEvent<ShowBrowserEvent>().Publish(new MoveAddressParameter() { Url = url, TrackerScript = modifyScript });

                // 원래 주소로 강제 다시 로드
                browser.MainFrame.LoadUrl(CurrentUrl);

            }
            else
            {
                _logger.LogDebug($"{response.Message}");
                throw new Exception("스크립트 정보를 읽어오는데 실패하였습니다.");
            }
        }

        protected override IResourceRequestHandler GetResourceRequestHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling)
        {
            return new UserAgentResourceRequestHandler();
        }
    }

    public class UserAgentResourceRequestHandler : ResourceRequestHandler
    {
        protected override CefReturnValue OnBeforeResourceLoad(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IRequestCallback callback)
        {
            var headers = request.Headers;
            headers["User-Agent"] = "PcPomissionSDK";
            request.Headers = headers;

            return CefReturnValue.Continue;
        }
    }

}
