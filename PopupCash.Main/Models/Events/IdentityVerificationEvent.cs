using PopupCash.Main.Models.Verifications;
using Prism.Events;

namespace PopupCash.Main.Models.Events
{
    public class IdentityVerificationEvent : PubSubEvent<ResponseIdentityVerification?>
    {
    }
}
