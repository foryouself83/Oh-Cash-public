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
                double left;
                double top;

                parentWindow.WindowStartupLocation = WindowStartupLocation.Manual;

                left = 10;
                top = 10;

                parentWindow.Left = left;
                parentWindow.Top = top;
            }
        }
    }
}
