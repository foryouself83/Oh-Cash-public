using System.Runtime.InteropServices;
using System.Windows;

namespace PopupCash.Contents.Models.Helpers
{
    internal static class WindowHelper
    {
        [DllImport("user32.dll")]
        private static extern int SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint flags);

        private const int SWP_SHOWWINDOW = 0x0040;
        private static readonly IntPtr HWND_TOP = new IntPtr(0);

        /// <summary>
        /// Control의 윈도우를 찾아서 최대화를 실행
        /// </summary>
        /// <param name="dependencyObject"></param>
        public static void AdjustWindowSizeToShowTaskbarFormControl(DependencyObject dependencyObject)
        {
            Window parentWindow = Window.GetWindow(dependencyObject);

            AdjustWindowSizeToShowTaskbar(parentWindow);
        }

        /// <summary>
        /// 윈도우의 최대화
        /// </summary>
        /// <param name="window"></param>
        public static void AdjustWindowSizeToShowTaskbar(Window window)
        {
            IntPtr handle = new System.Windows.Interop.WindowInteropHelper(window).Handle;

            // Get screen size excluding taskbar
            var screenRect = SystemParameters.WorkArea;

            // Set window position and size
            SetWindowPos(handle, HWND_TOP, 0, 0, (int)screenRect.Width, (int)screenRect.Height, SWP_SHOWWINDOW);
        }
    }
}
