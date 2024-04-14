using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using PopupCash.Common.Models.Dialogs.Parameters;
using PopupCash.Common.Models.Events;
using Prism.Events;
using Prism.Services.Dialogs;

namespace PopupCash.Common.ViewModels
{
    public partial class ActivityIndicatorViewModel : DialogViewModelBase
    {
        private readonly IEventAggregator _eventAggregator;

        public override event Action<IDialogResult>? RequestClose;

        [ObservableProperty]
        private ActivityIndicatorDialogParameter? _parameter;

        public ActivityIndicatorViewModel(IEventAggregator eventAggregator, ILogger<DialogViewModelBase> loggor) : base(loggor)
        {
            _eventAggregator = eventAggregator;


            _eventAggregator.GetEvent<ActivityIndicatorEvent>().Subscribe(OnReceiveActivityIndicatorEvent, ThreadOption.UIThread);
        }

        private void OnReceiveActivityIndicatorEvent(ActivityEventParameter parameter)
        {
            if (parameter.IsClose)
            {
                RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel, new DialogParameters()
                {
                }));
            }
        }

        //public override Task LoadedDialog(RoutedEventArgs args)
        //{
        //    if (args.OriginalSource is not DependencyObject dependencyObject) return Task.CompletedTask;

        //    var window = Window.GetWindow(dependencyObject);
        //    var contentBrowser = System.Windows.Application.Current.Windows.OfType<Window>().FirstOrDefault(x => x.Title.Equals("ContentBrowser"));

        //    window.WindowStartupLocation = WindowStartupLocation.CenterOwner;

        //    return Task.CompletedTask;
        //}


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
            if (parameters is ActivityIndicatorDialogParameter activityIndicatorParameter)
            {
                Parameter = activityIndicatorParameter;
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
    }
}
