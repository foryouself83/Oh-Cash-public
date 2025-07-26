using System.Windows;

using CefSharp;
using CefSharp.Wpf;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.Extensions.Logging;
using PopupCash.Account.Models.Cashs;
using PopupCash.Account.Models.Login;
using PopupCash.Common.Models.Events;
using PopupCash.Common.ViewModels;
using PopupCash.Contents.Models.Handlers.CefSharps;
using PopupCash.Contents.Models.Handlers.Scipts;
using PopupCash.Contents.Models.Helpers;
using PopupCash.Contents.Views;
using PopupCash.Controls.Buttons;
using PopupCash.Core.Models.Parameters;
using PopupCash.Database.Models.Services;
using Prism.Events;
using Prism.Services.Dialogs;

namespace PopupCash.Contents.ViewModels
{
    /// <summary>
    /// ChromiumWebBrowser View Model
    /// </summary>
    public partial class ContentBrowserDialogViewModel : DialogViewModelBase
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
        #endregion

        private IWpfWebBrowser? _webbrowser;

        private bool _isChangedAddress;

        private string _trackerScript;

        public override event Action<IDialogResult>? RequestClose;

        [ObservableProperty]
        private string _address;

        [ObservableProperty]
        private WindowState _windowState;

        public ContentBrowserDialogViewModel(IEventAggregator eventAggregator, IDialogService dialogService,
            ILoginService loginService, ICashService cashService,
            IAuthorizationDataService authorizationService, IUserDataService userDataService,
            ILogger<ContentBrowserDialogViewModel> loggor, ILoggerFactory loggerFactory) : base(loggor)
        {
            _eventAggregator = eventAggregator;
            _dialogService = dialogService;

            _loginService = loginService;
            _cashService = cashService;

            _authorizationService = authorizationService;
            _userDataService = userDataService;

            _loggerFactory = loggerFactory;

            _isChangedAddress = false;

            Address = string.Empty;
            _trackerScript = string.Empty;

            Title = "ContentBrowser";

            _eventAggregator.GetEvent<CloseDialogEvent>().Subscribe(OnReceiveCloseDialog, ThreadOption.UIThread);
        }


        private void OnReceiveCloseDialog(string name)
        {
            if (name is nameof(ContentBrowserDialog))
            {
                RequestClose?.Invoke(new DialogResult(ButtonResult.OK, new DialogParameters()
                {
                }));
            }
        }

        [RelayCommand]
        public void MinimumWindow(Window window)
        {
            window.WindowState = WindowState.Minimized;
        }

        [RelayCommand]
        public void RestoreWindow(ImageButton imageButton)
        {
            imageButton.IsImageToolge = !imageButton.IsImageToolge;

            Window parentWindow = Window.GetWindow(imageButton);

            if (imageButton.IsImageToolge)
            {
                WindowState = WindowState.Maximized;
                WindowHelper.AdjustWindowSizeToShowTaskbarFormControl(imageButton);
            }
            else
            {
                WindowState = WindowState.Normal;
            }
        }

        [RelayCommand]
        public void CloseWindow()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel, new DialogParameters()
            {
            }));
        }

        [RelayCommand]
        public void BrowserInitialized(object sender)
        {
            if (sender is not ChromiumWebBrowser webBrowser) { return; }

            _webbrowser = webBrowser;

            webBrowser.MenuHandler = new NoneMenuHandler();
            webBrowser.IsBrowserInitializedChanged += Webbrowser_IsBrowserInitializedChanged;
            webBrowser.LoadingStateChanged += OnLoadingStateChanged;
            webBrowser.AddressChanged += OnAddressChanged;
            webBrowser.ConsoleMessage += ConsoleMessage;
            webBrowser.FrameLoadStart += Webbrowser_FrameLoadStart;

            // IME 한글 입력 지원
            webBrowser.WpfKeyboardHandler = new CefSharp.Wpf.Experimental.WpfImeKeyboardHandler(webBrowser);

            // use java script object
            webBrowser.JavascriptObjectRepository.Settings.LegacyBindingEnabled = true;
            webBrowser.JavascriptObjectRepository.Register("HybridApp", new HybridAppScript(_eventAggregator, _dialogService,
                _loginService, _cashService,
                _authorizationService, _userDataService,
                _loggerFactory.CreateLogger<HybridAppScript>()),
                options: BindingOptions.DefaultBinder);

            // 새 탭 방지
            webBrowser.LifeSpanHandler = new SingleLifeSpanHandler();
        }

        private void Webbrowser_IsBrowserInitializedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is not IWpfWebBrowser webBrowser) { return; }

        }

        private void ConsoleMessage(object? sender, ConsoleMessageEventArgs e)
        {
            logger.LogTrace($"Console message : {e.Message}");
        }

        private async void Webbrowser_FrameLoadStart(object? sender, FrameLoadStartEventArgs e)
        {
            if (sender is not IWpfWebBrowser webBrowser) { return; }

            await semaphoreSlim.WaitAsync();

            try
            {
                while (!webBrowser.CanExecuteJavascriptInMainFrame)
                {
                    await Task.Delay(1);
                }

                // 변경된 스크립트 실행
                _ = webBrowser.EvaluateScriptAsync(_trackerScript).ConfigureAwait(false);

                logger.LogTrace($"Load start address : {Address}");
                logger.LogTrace($"modifyScript : {_trackerScript}");
                //_missonScript = string.Empty;

            }
            finally
            {
                semaphoreSlim.Release();
            }
        }

        private void OnAddressChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is not IWpfWebBrowser webBrowser) { return; }

            if (e.OldValue != e.NewValue)
            {
                _isChangedAddress = true;
                if (e.NewValue is string address)
                    Address = address;
            }
        }
        private SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1);
        private void OnLoadingStateChanged(object? sender, LoadingStateChangedEventArgs e)
        {
            if (sender is not IWpfWebBrowser webBrowser) { return; }

            if (e.IsLoading || !_isChangedAddress) { return; }
            _isChangedAddress = false;

            logger.LogDebug($"Loaded Url {Address}");
            //webBrowser.ShowDevTools();
        }

        public override bool CanCloseDialog()
        {
            return true;
        }

        public override void OnDialogClosed()
        {
            _webbrowser?.JavascriptObjectRepository.UnRegisterAll();
            CloseWindow();
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters is MoveAddressParameter moveAddress)
            {
                Address = moveAddress.Url;
                _trackerScript = moveAddress.TrackerScript;
            }
        }
    }
}
