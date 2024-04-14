using System.Windows;

using CefSharp;
using CefSharp.Wpf;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.Extensions.Logging;
using PopupCash.Account.Models.Cashs;
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
        private readonly ICashService _cashService;
        private readonly IAuthorizationDataService _authorizationService;
        #endregion

        private bool _isChangedAddress;

        private string _trackerScript;
        private string _missonScript;

        public override event Action<IDialogResult>? RequestClose;

        [ObservableProperty]
        private string _address;

        [ObservableProperty]
        private WindowState _windowState;

        public ContentBrowserDialogViewModel(IEventAggregator eventAggregator, IDialogService dialogService,
            ICashService cashService,
            IAuthorizationDataService authorizationService,
            ILogger<ContentBrowserDialogViewModel> loggor) : base(loggor)
        {
            _eventAggregator = eventAggregator;
            _dialogService = dialogService;

            _cashService = cashService;
            _authorizationService = authorizationService;

            _isChangedAddress = false;

            Address = string.Empty;
            _trackerScript = string.Empty;
            _missonScript = string.Empty;

            WindowWidth = 1440;
            WindowHeight = 1024;

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

            webBrowser.MenuHandler = new NoneMenuHandler();
            webBrowser.IsBrowserInitializedChanged += Webbrowser_IsBrowserInitializedChanged;
            webBrowser.LoadingStateChanged += OnLoadingStateChanged;
            webBrowser.AddressChanged += OnAddressChanged;
            webBrowser.ConsoleMessage += ConsoleMessage;
            webBrowser.FrameLoadStart += Webbrowser_FrameLoadStart;

            webBrowser.WpfKeyboardHandler = new CefSharp.Wpf.Experimental.WpfImeKeyboardHandler(webBrowser);

            webBrowser.JavascriptObjectRepository.Settings.LegacyBindingEnabled = true;
            webBrowser.JavascriptObjectRepository.Register("HybridApp", new HybridAppScript(_eventAggregator, _dialogService, _cashService, _authorizationService), options: BindingOptions.DefaultBinder);
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

                if (!string.IsNullOrEmpty(_missonScript))
                {
                    // 읽어들인 스크립트 내용에서 jsMission 변수를 변경하는 스크립트 작성
                    string script = $"var jsMission = {_missonScript}";
                    string? modifyScript = _trackerScript.ToString()?.Replace("var jsMission = {jsMission}", script);

                    // 변경된 스크립트 실행
                    _ = webBrowser.EvaluateScriptAsync(modifyScript).ConfigureAwait(false);

                    logger.LogTrace($"Load start address : {Address}");
                    logger.LogTrace($"modifyScript : {modifyScript}");
                    //_missonScript = string.Empty;
                }

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
            }
        }
        private SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1);
        private void OnLoadingStateChanged(object? sender, LoadingStateChangedEventArgs e)
        {
            if (sender is not IWpfWebBrowser webBrowser) { return; }

            if (e.IsLoading || !_isChangedAddress) { return; }
            _isChangedAddress = false;

            //webBrowser.ShowDevTools();
        }

        public override bool CanCloseDialog()
        {
            return true;
        }

        public override void OnDialogClosed()
        {
            CloseWindow();
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters is MoveAddressParameter moveAddress)
            {
                Address = moveAddress.Url;
                _trackerScript = moveAddress.TrackerScript;
                _missonScript = moveAddress.Script;
            }
        }
    }
}
