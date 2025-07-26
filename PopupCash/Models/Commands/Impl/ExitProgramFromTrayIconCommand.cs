using System.Windows;
using PopupCash.Common.Models.Commands.Impl;
using PopupCash.Database.Models.Locations;
using PopupCash.ViewModels;

namespace PopupCash.Models.Commands.Impl
{
    public class ExitProgramFromTrayIconCommand : TrayIconCommandBase<ExitProgramFromTrayIconCommand>
    {
        public override void Execute(object? parameter)
        {
            base.Execute(parameter);

            if (parameter is null || GetTaskbarWindow(parameter) is not Window window) return;

            var windowId = nameof(MainWindowViewModel).Replace("ViewModel", "");
            var command = new UpdateMainWindowPositionCommand(windowId);
            command.Execute(new WindowPosition(windowId, window.Left, window.Top));

            Environment.Exit(0);
        }


        public override bool CanExecute(object? parameter)
        {
            return true;
        }
    }
}
