using System.Windows;
using System.Windows.Input;

namespace PopupCash.Models.Commands.Impl
{
    public class ShowWindowFromTrayIconCommand : TrayIconCommandBase<ShowWindowFromTrayIconCommand>
    {
        public override void Execute(object? parameter)
        {
            if (parameter is null) return;
            if (GetTaskbarWindow(parameter) is not Window window) return;

            window.Show();
            window.Activate();
            window.Topmost = true;
            window.Topmost = false;
            window.Focus();

            CommandManager.InvalidateRequerySuggested();
        }


        public override bool CanExecute(object? parameter)
        {
            if (parameter is null) return false;
            if (GetTaskbarWindow(parameter) is not Window window) return false;

            return !window.IsVisible || !window.Topmost;
        }
    }
}
