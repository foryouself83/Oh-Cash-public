using PopupCash.Core.Models.Commands;
using PopupCash.Database.Models.Locations;
using PopupCash.Database.Models.Services;

namespace PopupCash.Common.Models.Commands.Impl
{
    // 윈도우 위치 설정 Command class
    public class SelectWindowPositionCommandAsync : ICommandSync<WindowPosition>
    {
        private readonly IWindowPositionDataService _windowPositionDataService;
        private readonly double _actualWidth;
        private readonly double _actualHeight;

        private bool _canExecute = true;

        public event EventHandler? CanExecuteChanged;

        public SelectWindowPositionCommandAsync(IWindowPositionDataService windowPositionDataService, double actualWidth, double actualHeight)
        {
            _windowPositionDataService = windowPositionDataService;

            _actualWidth = actualWidth;
            _actualHeight = actualHeight;
        }


        public bool CanExecute(object? parameter)
        {
            return _canExecute;
        }

        /// <summary>
        /// Database에 위치 값이 있는 경우 해당 위치 설정
        /// Database에 위치 값이 없는 경우 중앙에 위치 설정
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public WindowPosition? Execute(object? parameter)
        {
            if (parameter is not string windowId) throw new Exception("잘못된 파라메터 입니다.");

            if (_windowPositionDataService.SelectWindowPostion(windowId) is WindowPosition position)
            {

                return position;
            }
            else
            {
                var pos = SystemParameterHelper.GetScreenCenterPoint(_actualWidth, _actualHeight);
                return new WindowPosition(windowId, pos.X, pos.Y);
            }
        }
        public void RaiseCanExecuteChanged()
        {
            // CanExecuteChanged 이벤트가 null이 아니면 호출
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
