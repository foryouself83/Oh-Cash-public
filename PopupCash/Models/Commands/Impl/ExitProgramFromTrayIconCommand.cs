using System.Windows;
using System.Windows.Input;

namespace PopupCash.Models.Commands.Impl
{
    public class ExitProgramFromTrayIconCommand : TrayIconCommandBase<ExitProgramFromTrayIconCommand>
    {
        public override void Execute(object? parameter)
        {
            Environment.Exit(0);
            CommandManager.InvalidateRequerySuggested();
        }


        public override bool CanExecute(object? parameter)
        {
            if (parameter is null) return false;
            if (GetTaskbarWindow(parameter) is not Window window) return false;

            return window.IsVisible;
        }
    }
}
