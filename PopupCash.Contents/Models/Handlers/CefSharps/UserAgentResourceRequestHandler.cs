using CefSharp;
using CefSharp.Handler;

namespace PopupCash.Contents.Models.Handlers.CefSharps
{
    public class UserAgentResourceRequestHandler : ResourceRequestHandler
    {
        protected override CefReturnValue OnBeforeResourceLoad(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IRequestCallback callback)
        {
            var headers = request.Headers;
            headers["User-Agent"] = "PcPomissionSDK";
            request.Headers = headers;

            return CefReturnValue.Continue;
        }
    }

    public class UserAgentRequestHandler : RequestHandler
    {
        protected override IResourceRequestHandler GetResourceRequestHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling)
        {
            return new UserAgentResourceRequestHandler();
        }
    }

}
