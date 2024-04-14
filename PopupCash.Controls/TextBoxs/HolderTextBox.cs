using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PopupCash.Controls.TextBoxs
{
    public class HolderTextBox : TextBox
    {
        public static readonly DependencyProperty NextFocusElementProperty =
            DependencyProperty.Register("NextFocusElement", typeof(UIElement), typeof(HolderTextBox), new PropertyMetadata(null, OnNextFocusElementChanged));

        public static readonly DependencyProperty IsNextFocusEnableTextProperty =
            DependencyProperty.Register("IsNextFocusEnableText", typeof(bool), typeof(HolderTextBox), new PropertyMetadata(true));

        public static readonly DependencyProperty HolderTextProperty =
            DependencyProperty.Register("HolderText", typeof(string), typeof(HolderTextBox), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty HolderTextBrushProperty =
            DependencyProperty.Register("HolderTextBrush", typeof(SolidColorBrush), typeof(HolderTextBox), new PropertyMetadata(Brushes.Black));


        /// <summary>
        /// 다음 포커스할 UIElement
        /// </summary>
        public UIElement NextFocusElement
        {
            get { return (UIElement)GetValue(NextFocusElementProperty); }
            set { SetValue(NextFocusElementProperty, value); }
        }

        /// <summary>
        /// Tab 눌렀을 경우 포커스 상태가 가능한지 여부
        /// </summary>
        public bool IsNextFocusEnableText
        {
            get { return (bool)GetValue(IsNextFocusEnableTextProperty); }
            set { SetValue(IsNextFocusEnableTextProperty, value); }
        }

        /// <summary>
        /// HolderText
        /// </summary>
        public string HolderText
        {
            get { return (string)GetValue(HolderTextProperty); }
            set { SetValue(HolderTextProperty, value); }
        }

        /// <summary>
        /// HolderText Color
        /// </summary>
        public SolidColorBrush HolderTextBrush
        {
            get { return (SolidColorBrush)GetValue(HolderTextBrushProperty); }
            set { SetValue(HolderTextBrushProperty, value); }
        }

        static HolderTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HolderTextBox), new FrameworkPropertyMetadata(typeof(HolderTextBox)));
        }
        public HolderTextBox()
        {
            this.Loaded += HolderTextBoxLoaded;
            this.GotFocus += HolderTextBoxGotFocus;
        }

        private void HolderTextBoxGotFocus(object sender, RoutedEventArgs e)
        {
            this.SelectAll();
        }

        private void HolderTextBoxLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (sender is not TextBox idTextBox) return;

            if (string.IsNullOrEmpty(idTextBox.Text))
                this.Focus();
        }

        /// <summary>
        /// NextFocus 값이 변경되었을 경우 이벤트 처리
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnNextFocusElementChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not TextBox idTextBox) return;

            if (e.NewValue is not UIElement uiElement) return;

            if (!string.IsNullOrEmpty(idTextBox.Text))
                uiElement.Focus();
        }
    }
}
