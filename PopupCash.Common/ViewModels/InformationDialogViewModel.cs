using System.Windows;
using System.Windows.Threading;
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

        private DispatcherTimer _timer;

        [ObservableProperty]
        private InformationDialogParameter? _parameter;

        public InformationDialogViewModel(ILogger<InformationDialogViewModel> loggor) : base(loggor)
        {
            Title = "알림";
            _timer = new DispatcherTimer();
            _timer.Tick += ChangeOkButtonTextTick;
            _timer.Interval += TimeSpan.FromSeconds(1);
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

                if (Parameter.AutoExit > 0)
                {
                    _timer.Start();
                }
            }
        }

        private void ChangeOkButtonTextTick(object? sender, EventArgs e)
        {
            _timer.Stop();
            if (Parameter is not null)
            {
                Parameter.ConfirmButtonText = $"{--Parameter.AutoExit}초 후 자동으로 종료됩니다.";

                if (Parameter.AutoExit > 0)
                {
                    _timer.Start();
                }
                else
                {
                    RequestClose?.Invoke(new DialogResult(ButtonResult.OK, new DialogParameters()
                    {
                    }));
                }
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
