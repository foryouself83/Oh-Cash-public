using PopupCash.Core.Models.Commands;
using PopupCash.Database.Models.Locations;
using PopupCash.Database.Models.Services;

namespace PopupCash.Common.Models.Commands.Impl
{
    public class InsertOrUpdateWindowPositionCommand : ICommandVoid<WindowPosition>
    {
        private readonly IWindowPositionDataService _windowPositionDataService;

        private bool _canExecute = true;

        public event EventHandler? CanExecuteChanged;

        public InsertOrUpdateWindowPositionCommand(IWindowPositionDataService windowPositionDataService)
        {
            _windowPositionDataService = windowPositionDataService;
        }


        public bool CanExecute(object? parameter)
        {
            return _canExecute;
        }

        public void Execute(WindowPosition windowPosition)
        {
            if (_windowPositionDataService.SelectWindowPostion(windowPosition.Id) is WindowPosition)
            {
                _windowPositionDataService.UpdateWindowPostion(new WindowPosition(windowPosition.Id, windowPosition.Left, windowPosition.Top));
            }
            else
            {
                _windowPositionDataService.InsertWindowPostion(new WindowPosition(windowPosition.Id, windowPosition.Left, windowPosition.Top));
            }
        }
        public void RaiseCanExecuteChanged()
        {
            // CanExecuteChanged 이벤트가 null이 아니면 호출
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
