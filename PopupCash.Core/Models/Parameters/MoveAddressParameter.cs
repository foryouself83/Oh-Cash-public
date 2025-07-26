using Prism.Services.Dialogs;

namespace PopupCash.Core.Models.Parameters
{
    public class MoveAddressParameter : DialogParameters
    {
        public string Url;

        public string TrackerScript;


        public MoveAddressParameter()
        {
            Url = string.Empty;
            TrackerScript = string.Empty;
        }
    }
}
