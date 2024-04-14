using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using PopupCash.Common.Models.Dialogs.Parameters;
using Prism.Services.Dialogs;

namespace PopupCash.Common.ViewModels
{
    public partial class InformationDialogViewModel : DialogViewModelBase
    {
        public override event Action<IDialogResult>? RequestClose;

        [ObservableProperty]
        private InformationDialogParameter? _parameter;

        public InformationDialogViewModel(ILogger<InformationDialogViewModel> loggor) : base(loggor)
        {
            Title = "알림";
        }


        public override Task LoadedDialog(RoutedEventArgs args)
        {
            // 윈도우 위치를 재설정하지 않고 항상 owner center에서 출력
            return Task.CompletedTask;
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
            if (parameters is InformationDialogParameter parameter)
            {
                this.Parameter = parameter;
            }
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
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK, new DialogParameters()
            {
            }));
        }
    }
}
