using System.Windows.Controls;

namespace PopupCash.Main.Views
{
    /// <summary>
    /// Interaction logic for LoginDialog
    /// </summary>
    public partial class LoginDialog : UserControl
    {
        public LoginDialog()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((dynamic)this.DataContext).LoginPassword = ((PasswordBox)sender).SecurePassword; }
        }
    }
}
