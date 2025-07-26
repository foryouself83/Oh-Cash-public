using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace PopupCash.Contents.Presentation.Behaviours
{
    public class DefaultActivityIndicatorWindowPostionBehavior : Behavior<UserControl>
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
            Window parentWindow = Window.GetWindow(this.AssociatedObject);
            if (parentWindow != null)
            {
                parentWindow.WindowStartupLocation = WindowStartupLocation.Manual;

                if (parentWindow.Owner is Window ownedWindow)
                {
                    double left = ownedWindow.Left + (ownedWindow.Width - parentWindow.Width) / 2;
                    double top = ownedWindow.Top + (ownedWindow.Height - parentWindow.Height) / 2;

                    parentWindow.Left = left;
                    parentWindow.Top = top;
                }
            }
        }
    }
}
