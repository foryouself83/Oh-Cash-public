using System.Windows;

namespace PopupCash.Common.Models
{
    public static class SystemParameterHelper
    {
        public static Point GetScreenCenterPoint(double actualWidth, double actualHeight)
        {

            // 현재 활성화된 모니터의 작업 영역을 가져옵니다.
            double workingAreaWidth = SystemParameters.WorkArea.Width;
            double workingAreaHeight = SystemParameters.WorkArea.Height;

            // 윈도우의 좌상단 위치를 계산하여 설정합니다.
            var Left = (workingAreaWidth - actualWidth) / 2;
            var Top = (workingAreaHeight - actualHeight) / 2;

            return new Point(Left, Top);
        }
    }
}
