using System.Windows.Controls;

namespace PopupCash.Main.Views
{
    /// <summary>
    /// Interaction logic for JoinDialog
    /// </summary>
    public partial class JoinDialog : UserControl
    {
        public JoinDialog()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((dynamic)this.DataContext).JoinInfo.Password = ((PasswordBox)sender).SecurePassword; }
        }
        private void Confirm_PasswordBox_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((dynamic)this.DataContext).JoinInfo.PasswordConfirm = ((PasswordBox)sender).SecurePassword; }
        }

        private void HolderTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            foreach (char c in e.Text)
            {
                if (!char.IsDigit(c)) // 입력된 문자가 숫자가 아니면
                {
                    e.Handled = true; // 이벤트를 처리했다고 표시하여 입력을 거부
                    return;
                }
            }
        }
    }
}
