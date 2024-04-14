using System.Windows;
using System.Windows.Input;
using PopupCash.ViewModels;

namespace PopupCash.Models.Commands.Impl
{
    public class OpenSourceInformationTrayIconCommand : TrayIconCommandBase<OpenSourceInformationTrayIconCommand>
    {
        public override void Execute(object? parameter)
        {
            if (parameter is null) return;
            if (GetTaskbarWindow(parameter) is not Window window) return;
            if (window.DataContext is not MainWindowViewModel viewModel) return;

            viewModel.ShowOpenSourceDialog();

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
