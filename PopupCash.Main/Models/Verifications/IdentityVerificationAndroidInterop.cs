using System.Diagnostics;
using Newtonsoft.Json;
using PopupCash.Main.Models.Events;
using Prism.Events;

namespace PopupCash.Main.Models.Verifications
{
    public class IdentityVerificationAndroidInterop
    {
        private readonly IEventAggregator _eventAggregator;
        public IdentityVerificationAndroidInterop(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }
        public void setMessage(string message)
        {
            // 받은 메시지 처리
            Debug.WriteLine($"Received message from JavaScript: {message}");
            var verification = JsonConvert.DeserializeObject<ResponseIdentityVerification>(message);

            _eventAggregator.GetEvent<IdentityVerificationEvent>().Publish(verification);

        }
    }
}
