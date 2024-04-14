using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace PopupCash.Contents.Presentation.Behaviours
{
    public class DefaultAccumulateWindowPostionBehavior : Behavior<UserControl>
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
                double left;
                double top;

                parentWindow.WindowStartupLocation = WindowStartupLocation.Manual;

                if (parentWindow.Owner is not Window ownedWindow) return;

                left = ownedWindow.Left;
                top = ownedWindow.Top + ownedWindow.ActualHeight + 100;

                parentWindow.Left = left;
                parentWindow.Top = top;
            }

            //Window parentWindow = Window.GetWindow(this.AssociatedObject);
            //if (parentWindow != null)
            //{
            //    var screenRect = SystemParameters.WorkArea;

            //    var left = screenRect.Width - 400;
            //    var top = screenRect.Height - 800;

            //    parentWindow.Left = left;
            //    parentWindow.Top = top;
            //}
        }
    }
}
