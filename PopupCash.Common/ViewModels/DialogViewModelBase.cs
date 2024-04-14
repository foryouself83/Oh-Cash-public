using System.Windows;
using System.Windows.Input;

using CommunityToolkit.Mvvm.Input;

using Microsoft.Extensions.Logging;

using PopupCash.Common.Models.Commands.Impl;
using PopupCash.Database.Models.Locations;

using Prism.Services.Dialogs;

namespace PopupCash.Common.ViewModels
{
    public abstract partial class DialogViewModelBase : WindowViewModelBase, IDialogAware
    {
        public abstract event Action<IDialogResult> RequestClose;

        public DialogViewModelBase(ILogger<DialogViewModelBase> loggor) : base(loggor)
        {
        }

        public abstract bool CanCloseDialog();
        public abstract void OnDialogClosed();
        public abstract void OnDialogOpened(IDialogParameters parameters);

        /// <summary>
        /// 윈도우가 로드 때 윈도우 위치 정보를 설정
        /// </summary>
        /// <param name="args"></param>
        [RelayCommand]
        public virtual Task LoadedDialog(RoutedEventArgs args)
        {
            if (args.OriginalSource is not DependencyObject dependencyObject) return Task.CompletedTask;
            if (GetWindowPosition() is not WindowPosition windowPosition) return Task.CompletedTask;

            var window = Window.GetWindow(dependencyObject);

            // 데이터 베이스에 위치 정보가 있는 경우 위치 설정
            window.WindowStartupLocation = WindowStartupLocation.Manual;
            window.Left = WindowLeft = windowPosition.Left;
            window.Top = WindowTop = windowPosition.Top;

            return Task.CompletedTask;
        }

        /// <summary>
        /// Left Button Down 시 윈도우 위치를 설정한다.
        /// Dialog Service를 사용하는 UserControl 일 경우에만 사용
        /// </summary>
        /// <param name="args"></param>
        [RelayCommand]
        public virtual void MouseLeftButtonDownDialog(MouseButtonEventArgs args)
        {
            if (args.OriginalSource is not DependencyObject dependencyObject) return;

            var window = Window.GetWindow(dependencyObject);

            if (WindowLeft == window.Left && WindowTop == window.Top) return;

            WindowLeft = window.Left;
            WindowTop = window.Top;

            logger.LogDebug($"{windowId} set window position. ({WindowLeft}, {WindowTop})");

            var command = new UpdateMainWindowPositionCommand(windowId);
            command.Execute(new WindowPosition(windowId, window.Left, window.Top));
        }
    }
}
