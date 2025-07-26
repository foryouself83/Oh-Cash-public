using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace PopupCash.Contents.Presentation.Behaviours
{
    public class DefaultBrowerWindowPostionBehavior : Behavior<UserControl>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += AssociatedObject_Loaded;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.Loaded -= AssociatedObject_Loaded;
        }

        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            CenterWindow();
        }

        private void CenterWindow()
        {
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;

            Window parentWindow = Window.GetWindow(this.AssociatedObject);
            if (parentWindow != null)
            {
                var mainWindow = System.Windows.Application.Current.MainWindow;
                var screen = GetCurrentScreen(mainWindow);

                parentWindow.WindowStartupLocation = WindowStartupLocation.Manual;

                var left = mainWindow.Left + mainWindow.ActualWidth + 10;

                if (left >= screen.WorkingArea.Left + screen.WorkingArea.Width - 400)
                {
                    left = mainWindow.Left - 10 - parentWindow.ActualWidth;
                }

                parentWindow.Left = left;
                parentWindow.Top = mainWindow.Top;
            }
        }
        private System.Windows.Forms.Screen GetCurrentScreen(Window window)
        {
            // Get the handle for the window
            var windowInteropHelper = new System.Windows.Interop.WindowInteropHelper(window);
            IntPtr windowHandle = windowInteropHelper.Handle;

            // Get the screen that contains the window
            return System.Windows.Forms.Screen.FromHandle(windowHandle);
        }
    }
}
