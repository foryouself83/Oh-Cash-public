using System.Windows;
using CefSharp;
using CefSharp.Wpf;
using Microsoft.Xaml.Behaviors;

namespace PopupCash.Contents.Presentation.Behaviours
{
    public class ChromiumUrlLoadedBehaviour : Behavior<ChromiumWebBrowser>
    {
        public static readonly DependencyProperty UrlProperty =
            DependencyProperty.Register("Url", typeof(string), typeof(ChromiumUrlLoadedBehaviour), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty AlertMsgProperty =
            DependencyProperty.Register("AlertMsg", typeof(string), typeof(ChromiumUrlLoadedBehaviour), new PropertyMetadata(string.Empty));

        private bool isChangedAddress;

        public string Url
        {
            get { return (string)GetValue(UrlProperty); }
            set { SetValue(UrlProperty, value); }
        }

        public string AlertMsg
        {
            get { return (string)GetValue(AlertMsgProperty); }
            set { SetValue(AlertMsgProperty, value); }
        }

        protected override void OnAttached()
        {
            //AssociatedObject.LoadHandler = new CustomLoadHandler(Url, AlertMsg);
            AssociatedObject.LoadingStateChanged += OnLoadingStateChanged;
            AssociatedObject.AddressChanged += OnAddressChanged;
        }

        private void OnAddressChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != e.NewValue)
                isChangedAddress = true;
        }

        private void OnLoadingStateChanged(object? sender, LoadingStateChangedEventArgs e)
        {
            if (sender is not IWpfWebBrowser webbrowser) { return; }
            webbrowser.Dispatcher.BeginInvoke(new Action(async () =>
            {
                if (e.IsLoading || !isChangedAddress) { return; }

                if (webbrowser.GetMainFrame().Url.Contains(Url))
                {
                    await webbrowser.EvaluateScriptAsync($"alert('{AlertMsg}');");
                    isChangedAddress = false;
                }
            }));
        }

        protected override void OnDetaching()
        {
            AssociatedObject.LoadingStateChanged -= OnLoadingStateChanged;
            AssociatedObject.AddressChanged -= OnAddressChanged;
        }
    }
}
