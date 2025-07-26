using CefSharp;

namespace PopupCash.Contents.Models.Handlers.CefSharps
{
    /// <summary>
    /// Tab, new window를 만들지 않는 핸들러
    /// </summary>
    public class SingleLifeSpanHandler : ILifeSpanHandler
    {
        public bool DoClose(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            return true;
        }

        public void OnAfterCreated(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            return;
        }

        public void OnBeforeClose(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            return;
        }

        // Load new URL (when clicking a link with target=_blank) in the same frame
        public bool OnBeforePopup(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures, IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser? newBrowser)
        {
            browser.MainFrame.LoadUrl(targetUrl);
            newBrowser = null!;
            return true;
        }
    }
}
