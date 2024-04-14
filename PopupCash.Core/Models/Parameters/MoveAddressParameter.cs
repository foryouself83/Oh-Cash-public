using Prism.Services.Dialogs;

namespace PopupCash.Core.Models.Parameters
{
    public class MoveAddressParameter : DialogParameters
    {
        public string Url;

        public string TrackerScript;

        public string Script;


        public MoveAddressParameter()
        {
            Url = string.Empty;
            TrackerScript = string.Empty;
            Script = string.Empty;
        }
    }
}
