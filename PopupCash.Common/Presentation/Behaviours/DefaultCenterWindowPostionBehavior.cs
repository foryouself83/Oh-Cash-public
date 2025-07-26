using System.Windows;
using System.Windows.Controls;

using Microsoft.Xaml.Behaviors;

namespace PopupCash.Common.Presentation.Behaviours
{
    public class DefaultCenterWindowPostionBehavior : Behavior<UserControl>
    {
        public static readonly DependencyProperty OwnedWindowTitleProperty =
            DependencyProperty.Register("OwnedWindowTitle", typeof(string), typeof(DefaultCenterWindowPostionBehavior), new PropertyMetadata(""));

        public string OwnedWindowTitle
        {
            get { return (string)GetValue(OwnedWindowTitleProperty); }
            set { SetValue(OwnedWindowTitleProperty, value); }
        }

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
            if (parentWindow != null && !string.IsNullOrEmpty(OwnedWindowTitle))
            {
                parentWindow.WindowStartupLocation = WindowStartupLocation.Manual;

                if (System.Windows.Application.Current.Windows.OfType<Window>().FirstOrDefault(x => x.Title.Equals(OwnedWindowTitle)) is not Window ownedWindow) return;

                // Calculate the center position of the owned window
                double ownedWindowCenterX = ownedWindow.Left + (ownedWindow.Width / 2);
                double ownedWindowCenterY = ownedWindow.Top + (ownedWindow.Height / 2);

                // Calculate the top-left position of the parent window to center it on the owned window
                double left = ownedWindowCenterX - (parentWindow.Width / 2);
                double top = ownedWindowCenterY - (parentWindow.Height / 2);

                // Set the calculated position
                parentWindow.Left = left;
                parentWindow.Top = top;
            }
        }
    }
}
