using System.Windows;
using System.Windows.Input;

namespace PopupCash.Models.Commands.Impl
{
    public class HideWindowFromTrayIconCommand : TrayIconCommandBase<HideWindowFromTrayIconCommand>
    {
        public override void Execute(object? parameter)
        {
            base.Execute(parameter);

            var windows = System.Windows.Application.Current.Windows.OfType<Window>();

            foreach (var window in windows)
            {
                if (!window.Equals(System.Windows.Application.Current.MainWindow))
                    window.Close();
                else
                    window.Hide();
            }
            //if (parameter is null) return;
            //if (GetTaskbarWindow(parameter) is not Window window) return;

            //window.Hide();
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
