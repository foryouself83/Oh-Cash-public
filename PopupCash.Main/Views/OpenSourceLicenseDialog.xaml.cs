using System.Diagnostics;
using System.Windows.Controls;

namespace PopupCash.Main.Views
{
    /// <summary>
    /// Interaction logic for OpenSourceLicenseDialog
    /// </summary>
    public partial class OpenSourceLicenseDialog : UserControl
    {
        public OpenSourceLicenseDialog()
        {
            InitializeComponent();
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo("cmd", $"/c start {e.Uri.AbsoluteUri}") { CreateNoWindow = true });
            e.Handled = true;
        }
    }
}
