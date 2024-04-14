using System.Windows;

namespace PopupCash.Core.Presentation.Behavior
{
    public class WindowOwnerBehavior
    {
        public static readonly DependencyProperty WindowOwnerFromTitleProperty = DependencyProperty.RegisterAttached("WindowOwnerFromTitle", typeof(string)
            , typeof(WindowOwnerBehavior), new PropertyMetadata("MainWindow", ChangedWindowOwner));

        public static string GetWindowOwnerFromTitle(DependencyObject dependencyObject)
        {
            return (string)dependencyObject.GetValue(WindowOwnerFromTitleProperty);
        }

        public static void SetWindowOwnerFromTitle(DependencyObject dependencyObject, string value)
        {
            dependencyObject.SetValue(WindowOwnerFromTitleProperty, value);
        }

        private static void ChangedWindowOwner(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not Window window) return;
            if (e.NewValue is not string title) return;

            if (System.Windows.Application.Current.Windows.OfType<Window>().FirstOrDefault(x => x.Title.Equals(title)) is not Window targetWindow) return;

            window.Owner = targetWindow;

        }
    }
}
