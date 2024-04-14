using Prism.Services.Dialogs;

namespace PopupCash.Core.Models.Parameters
{
    public class StringTextParameter : DialogParameters
    {
        public string Text;

        public StringTextParameter()
        {
            Text = string.Empty;
        }
    }
}
