using System.Windows;

using CefSharp;
using CefSharp.Wpf;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.Extensions.Logging;

using PopupCash.Account.Models.Cashs;
using PopupCash.Account.Models.Login;
using PopupCash.Common.ViewModels;
using PopupCash.Contents.Models.Events;
using PopupCash.Contents.Models.Handlers.CefSharps;
using PopupCash.Contents.Models.Handlers.Scipts;
using PopupCash.Core.Models.Parameters;
using PopupCash.Database.Models.Services;
using PopupCash.Database.Models.Users;

using Prism.Events;
using Prism.Services.Dialogs;

namespace PopupCash.Contents.ViewModels
{
    /// <summary>
    /// 적립 윈도우 ViewModel
    /// </summary>
    public partial class AccumulateBrowserDialogVIewModel : DialogViewModelBase
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

        private readonly ILoggerFactory _loggerFactory;

        private IWpfWebBrowser? _webbrowser;
        #endregion

        private string _originUrl;

        [ObservableProperty]
        private string _address;

        public override event Action<IDialogResult>? RequestClose;

        public AccumulateBrowserDialogVIewModel(IEventAggregator eventAggregator, IDialogService dialogService,
            ILoginService loginService, ICashService cashService,
            IAuthorizationDataService authorizationService, IUserDataService userDataService,
            ILogger<AccumulateBrowserDialogVIewModel> loggor, ILoggerFactory loggerFactory) : base(loggor)
        {
            _eventAggregator = eventAggregator;
            _dialogService = dialogService;

            _loginService = loginService;
            _cashService = cashService;

            _authorizationService = authorizationService;
            _userDataService = userDataService;

            _loggerFactory = loggerFactory;

            _address = string.Empty;

            _originUrl = string.Empty;

            Title = "적립 리스트";

            Address = string.Empty;

            _eventAggregator.GetEvent<RefreshMissionBrowserEvent>().Subscribe(OnReceiveRefreshMissionBrowser, ThreadOption.UIThread);

        }

        private void OnReceiveRefreshMissionBrowser(MoveAddressParameter parameter)
        {
            logger.LogDebug("Run RefreshMissionBrowserEvent");
            //browser.Reload();
            Address = _originUrl;

            Application.Current.Dispatcher.BeginInvoke(() =>
            {
                _webbrowser.Reload();
            });
        }

        [RelayCommand]
        public void Cancel()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel, new DialogParameters()
            {
            }));
        }
        [RelayCommand]
        public void BrowserInitialized(object sender)
        {
            if (sender is not ChromiumWebBrowser webbrowser) { return; }

            _webbrowser = webbrowser;

            webbrowser.RequestHandler = new UserAgentWithScriptRequestHandler(_eventAggregator, _dialogService, _loggerFactory.CreateLogger<UserAgentWithScriptRequestHandler>());
            webbrowser.MenuHandler = new NoneMenuHandler();
            webbrowser.FrameLoadStart += Webbrowser_FrameLoadStart;
            webbrowser.LoadingStateChanged += OnLoadingStateChanged;
            webbrowser.AddressChanged += OnAddressChanged;

            webbrowser.WpfKeyboardHandler = new CefSharp.Wpf.Experimental.WpfImeKeyboardHandler(webbrowser);

            webbrowser.JavascriptObjectRepository.Settings.LegacyBindingEnabled = true;
            webbrowser.JavascriptObjectRepository.Register("HybridApp", new HybridAppScript(_eventAggregator, _dialogService,
                _loginService, _cashService,
                _authorizationService, _userDataService,
                _loggerFactory.CreateLogger<HybridAppScript>()),
                options: BindingOptions.DefaultBinder);
        }

        private void Webbrowser_FrameLoadStart(object? sender, FrameLoadStartEventArgs e)
        {
            if (sender is not ChromiumWebBrowser webbrowser) { return; }

            if (webbrowser.RequestHandler is not UserAgentWithScriptRequestHandler userAgentRequestHandler) return;
            if (!string.IsNullOrEmpty(userAgentRequestHandler.CurrentUrl)) return;

            userAgentRequestHandler.CurrentUrl = _originUrl;
        }

        public override Task LoadedDialog(RoutedEventArgs args)
        {
            if (_authorizationService.SelectLastestAuthorization() is not Authorization authorization) throw new Exception("액세스 토큰 값이 없습니다.");

            if (!authorization.Policy)
            {
                Application.Current.Dispatcher.BeginInvoke(() =>
                {
                    _dialogService.ShowDialog("DataCollectionInstructionsDialog",
                    (result) =>
                    {
                        if (result is IDialogResult dialogResult &&
                            dialogResult.Result != ButtonResult.OK)
                        {
                            RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
                        }
                    });
                });
            }

            return Task.CompletedTask;
        }

        private void OnAddressChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
        }

        private void OnLoadingStateChanged(object? sender, LoadingStateChangedEventArgs e)
        {
        }

        public override bool CanCloseDialog()
        {
            return true;
        }

        public override void OnDialogClosed()
        {
            _webbrowser?.JavascriptObjectRepository.UnRegisterAll();
            Cancel();
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters is MoveAddressParameter moveAddress)
            {
                _originUrl = moveAddress.Url;
                Address = moveAddress.Url;

            }
        }
    }
}
