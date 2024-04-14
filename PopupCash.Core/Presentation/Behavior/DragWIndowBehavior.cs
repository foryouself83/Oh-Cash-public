using System.Windows;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace PopupCash.Core.Presentation.Behavior
{
    public class DragWindowBehavior : Behavior<UIElement>
    {
        protected override void OnAttached()
        {
            AssociatedObject.MouseLeftButtonDown += AssociatedObjectMouseLeftButtonDown;
        }


        protected override void OnDetaching()
        {
            AssociatedObject.MouseLeftButtonDown -= AssociatedObjectMouseLeftButtonDown;
        }

        private void AssociatedObjectMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (sender is FrameworkElement element)
                {
                    var window = Window.GetWindow(element);
                    window.DragMove();
                }
            }
        }
    }
}
