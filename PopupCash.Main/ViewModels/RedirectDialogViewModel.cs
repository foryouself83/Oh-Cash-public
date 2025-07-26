using System.Text;
using System.Web;
using System.Windows;
using CefSharp;
using CefSharp.Wpf;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using PopupCash.Account.Models.Authenthications.Impl;
using PopupCash.Account.Models.Login;
using PopupCash.Common.ViewModels;
using PopupCash.Contents.Models.Handlers.CefSharps;
using Prism.Services.Dialogs;

namespace PopupCash.Main.ViewModels
{
    public partial class RedirectDialogViewModel : DialogViewModelBase
    {
        private readonly ILoginService _loginService;

        public override event Action<IDialogResult>? RequestClose;

        [ObservableProperty]
        private string _address;

        public RedirectDialogViewModel(ILoginService loginService, ILogger<DialogViewModelBase> loggor) : base(loggor)
        {
            _loginService = loginService;

            Address = _loginService.GetAuthCodeUrl();

            WindowWidth = 580;
            WindowHeight = 800;
        }

        #region IDialogAware Methods
        public override bool CanCloseDialog()
        {
            return true;
        }

        public override void OnDialogClosed()
        {
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
        }

        [RelayCommand]
        public void Cancel()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel, new DialogParameters()
            {
            }));
        }
        #endregion

        [RelayCommand]
        public void BrowserInitialized(object sender)
        {
            if (sender is not ChromiumWebBrowser webbrowser) { return; }

            webbrowser.MenuHandler = new NoneMenuHandler();
            webbrowser.LoadingStateChanged += OnLoadingStateChanged;
            webbrowser.AddressChanged += OnAddressChanged;

            webbrowser.WpfKeyboardHandler = new CefSharp.Wpf.Experimental.WpfImeKeyboardHandler(webbrowser);
        }
        private async void OnLoadingStateChanged(object? sender, LoadingStateChangedEventArgs e)
        {
            if (e.Browser is not IBrowser browser) return;

            logger.LogDebug($"{e.IsLoading}");
            if (e.Browser is not null &&
                e.Browser.MainFrame is not null &&
                !string.IsNullOrEmpty(e.Browser.MainFrame.Url))
            {
                Uri uri = new Uri(e.Browser.MainFrame.Url);
                var query = HttpUtility.ParseQueryString(uri.Query);

                if (query.AllKeys.Contains("error"))
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var key in query.AllKeys)
                    {
                        sb.Append($"key: {key}, ");
                        sb.AppendLine($"value:{query[key]}");
                    }

                    logger.LogDebug($"{sb.ToString()}");
                    await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel, new DialogParameters()
                        {
                        }));
                    }));
                    return;
                }
            }

            if (await _loginService.GetTokenAsync(browser.MainFrame.Url) is not AuthTokenInfo authTokenInfo) return;

            var token = authTokenInfo.AccessToken;

            await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                RequestClose?.Invoke(new DialogResult(ButtonResult.OK, new DialogParameters($"accessToken={token}")
                {
                }));
            }));
        }

        private void OnAddressChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != e.NewValue)
            {
                if (e.NewValue is not string newURI) return;

                Address = newURI;
            }
        }
    }
}
