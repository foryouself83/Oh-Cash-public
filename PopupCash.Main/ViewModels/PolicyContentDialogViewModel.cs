using CefSharp;
using CefSharp.Wpf;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using PopupCash.Common.ViewModels;
using PopupCash.Contents.Models.Handlers.CefSharps;
using PopupCash.Core.Models.Parameters;
using Prism.Services.Dialogs;

namespace PopupCash.Main.ViewModels
{
    public partial class PolicyContentDialogViewModel : DialogViewModelBase
    {
        public override event Action<IDialogResult>? RequestClose;

        [ObservableProperty]
        private string _htmlText;

        public PolicyContentDialogViewModel(ILogger<PolicyContentDialogViewModel> loggor) : base(loggor)
        {
            HtmlText = string.Empty;

            WindowWidth = 480;
            WindowHeight = 900;

            Title = "Policy";
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
            if (parameters is StringTextParameter policyString)
                HtmlText = policyString.Text;
        }

        [RelayCommand]
        public void Cancel()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel, new DialogParameters()
            {

            }));
        }
        public bool CanJoinExcute()
        {
            return !IsBusy;
        }
        #endregion

        [RelayCommand]
        public void BrowserInitialized(object sender)
        {
            if (sender is not ChromiumWebBrowser webbrowser) { return; }

            webbrowser.MenuHandler = new NoneMenuHandler();
        }
        [RelayCommand]
        public void BrowserLoaded(object sender)
        {
            if (sender is not ChromiumWebBrowser webbrowser) { return; }

            webbrowser.LoadHtml(HtmlText, "http://Enliple.com/");
        }
    }
}
