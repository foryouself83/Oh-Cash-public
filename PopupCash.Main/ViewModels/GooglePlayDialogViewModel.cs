using System.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using PopupCash.Common.ViewModels;
using Prism.Services.Dialogs;

namespace PopupCash.Main.ViewModels
{
    public partial class GooglePlayDialogViewModel : DialogViewModelBase
    {
        public override event Action<IDialogResult>? RequestClose;


        public GooglePlayDialogViewModel(ILogger<GooglePlayDialogViewModel> loggor) : base(loggor)
        {
            Title = "N페이 교환";

            // 윈도우 크기
            WindowWidth = 452;
            WindowHeight = 214;
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
        public void Confirm()
        {
            //Process.Start("https://play.google.com/store/apps/details?id=com.popupcash&hl=ko&gl=US");

            Process.Start(new ProcessStartInfo("cmd", $"/c start https://play.google.com/store/apps/details?id=com.popupcash&hl=ko&gl=US") { CreateNoWindow = true });

            RequestClose?.Invoke(new DialogResult(ButtonResult.OK, new DialogParameters()
            {
            }));
        }
    }
}
