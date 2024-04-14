using System.Windows;
using CefSharp;
using CefSharp.Wpf;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using PopupCash.Common.ViewModels;
using PopupCash.Contents.Models.Handlers.CefSharps;
using PopupCash.Main.Models.Events;
using PopupCash.Main.Models.Verifications;
using Prism.Events;
using Prism.Services.Dialogs;

namespace PopupCash.Main.ViewModels
{
    public partial class IdentityVerificationDialogViewModel : DialogViewModelBase
    {
        #region Prism
        private readonly IEventAggregator _eventAggregator;
        #endregion

        #region Interops

        #endregion
        private IdentityVerificationAndroidInterop _identityVerificationAndroidInterop;

        public override event Action<IDialogResult>? RequestClose;

        [ObservableProperty]
        private string _address;

        public IdentityVerificationDialogViewModel(IEventAggregator eventAggregator, ILogger<IdentityVerificationDialogViewModel> loggor) : base(loggor)
        {
            _eventAggregator = eventAggregator;

            Title = "본인 인증";

            // 윈도우 크기
            WindowWidth = 528;
            WindowHeight = 900;

            // 본인 인증 주소
            Address = "https://popupcash.co.kr/phone/verifyView";

            _identityVerificationAndroidInterop = new IdentityVerificationAndroidInterop(eventAggregator);

            _eventAggregator.GetEvent<IdentityVerificationEvent>().Subscribe(OnReceivedIdentityVerification, ThreadOption.UIThread);
        }

        private void OnReceivedIdentityVerification(ResponseIdentityVerification? verification)
        {
            if (verification is null) return;
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK, new DialogParameters()
            {
            }));

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

            webbrowser.JavascriptObjectRepository.Settings.LegacyBindingEnabled = true;
            webbrowser.JavascriptObjectRepository.Register("android", _identityVerificationAndroidInterop, options: BindingOptions.DefaultBinder);

            //webbrowser.RegisterJsObject("android", new AndroidInterop());
        }
        private void OnLoadingStateChanged(object? sender, LoadingStateChangedEventArgs e)
        {
            if (sender is not IWpfWebBrowser webbrowser) { return; }
        }

        private void OnAddressChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != e.NewValue)
            {
            }
        }
    }
}
