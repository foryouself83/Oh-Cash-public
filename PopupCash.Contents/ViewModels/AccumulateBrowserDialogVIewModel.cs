using System.Windows;

using CefSharp;
using CefSharp.Wpf;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;
using PopupCash.Account.Models.Cashs;
using PopupCash.Common.ViewModels;
using PopupCash.Contents.Models.Events;
using PopupCash.Contents.Models.Handlers.CefSharps;
using PopupCash.Contents.Models.Handlers.Scipts;
using PopupCash.Core.Models.Parameters;
using PopupCash.Database.Models.Services;
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

        private readonly ICashService _cashService;
        private readonly IAuthorizationDataService _authorizationService;
        #endregion

        private bool _isChangedAddress;

        private string _moveUrl;
        private string _originUrl;
        private string _missonScript;

        [ObservableProperty]
        private string _address;

        public override event Action<IDialogResult>? RequestClose;

        public AccumulateBrowserDialogVIewModel(IEventAggregator eventAggregator, IDialogService dialogService,
            ICashService cashService,
            IAuthorizationDataService authorizationService,
            ILogger<AccumulateBrowserDialogVIewModel> loggor) : base(loggor)
        {
            _eventAggregator = eventAggregator;
            _dialogService = dialogService;

            _cashService = cashService;
            _authorizationService = authorizationService;

            _address = string.Empty;

            _originUrl = string.Empty;
            _moveUrl = string.Empty;
            _missonScript = string.Empty;

            WindowWidth = 452;
            WindowHeight = 600;

            Title = "적립 리스트";

            Address = string.Empty;

            _eventAggregator.GetEvent<MissionScriptEvent>().Subscribe(OnReceiveMissionScript);
        }

        private void OnReceiveMissionScript(ResponseMissonScript? script)
        {
            _missonScript = JsonConvert.SerializeObject(script);
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

            webbrowser.RequestHandler = new UserAgentRequestHandler();
            webbrowser.MenuHandler = new NoneMenuHandler();
            webbrowser.LoadingStateChanged += OnLoadingStateChanged;
            webbrowser.AddressChanged += OnAddressChanged;

            webbrowser.WpfKeyboardHandler = new CefSharp.Wpf.Experimental.WpfImeKeyboardHandler(webbrowser);

            webbrowser.JavascriptObjectRepository.Settings.LegacyBindingEnabled = true;
            webbrowser.JavascriptObjectRepository.Register("HybridApp", new HybridAppScript(_eventAggregator, _dialogService, _cashService, _authorizationService), options: BindingOptions.DefaultBinder);
        }

        public override Task LoadedDialog(RoutedEventArgs args)
        {
            base.LoadedDialog(args);

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
            return Task.CompletedTask;
        }

        private void OnAddressChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != e.NewValue)
            {
                if (e.NewValue.ToString() is string newValue)
                {
                    if (Address.Equals(newValue)) return;

                    Address = newValue;

                    if (!newValue.Contains("https://pomission.com/common/view/missionList"))
                    {
                        _isChangedAddress = true;
                        _moveUrl = newValue;

                    }

                }
            }
        }

        private async void OnLoadingStateChanged(object? sender, LoadingStateChangedEventArgs e)
        {
            if (sender is not IWpfWebBrowser webBrowser) { return; }

            if (e.IsLoading || !_isChangedAddress) { return; }

            _isChangedAddress = false;

            string readScript = "fetch('https://pomission.com/attach/script/tracker.js')" +
                        ".then(response => response.text())" +
                        ".then(data => { return data; });";

            if (await webBrowser.EvaluateScriptAsync(readScript) is not JavascriptResponse scriptResult) throw new Exception("추적 스크립트 정보가 없습니다.");

            webBrowser.Dispatcher.Invoke(() =>
            {
                _dialogService.Show("ContentBrowserDialog", new MoveAddressParameter() { Url = _moveUrl, TrackerScript = scriptResult.Result.ToString()!, Script = _missonScript },
                new Action<IDialogResult>((result) =>
                {
                }));
            });
            Address = _originUrl;
        }

        public override bool CanCloseDialog()
        {
            return true;
        }

        public override void OnDialogClosed()
        {
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
