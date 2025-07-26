using CefSharp;
using PopupCash.Main.ViewModels;
using PopupCash.Main.Views;

using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace PopupCash.Main.Models.Modules
{
    /// <summary>
    /// 메인화면 컨텐츠 모듈
    /// </summary>
    public class MainModuleContent : IModule
    {
        public MainModuleContent()
        {
            if (!CefSharp.Cef.IsInitialized)
            {

                var cachePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PopupCash\\CefSharp\\Cache");
                // 캐시 폴더 삭제
                //Directory.Delete(cachePath, true);

                var settings = new CefSharp.Wpf.CefSettings()
                {
                    //LogSeverity = LogSeverity.Disable,
                    Locale = "ko-KR", // 예: 한국어(Korea)로 설정 

                    LocalesDirPath = System.IO.Path.Combine(Environment.CurrentDirectory, "locales"),
                    ResourcesDirPath = Environment.CurrentDirectory,

                    //By default CefSharp will use an in-memory cache, you need to specify a Cache Folder to persist data
                    CachePath = cachePath,
                };
                // 더 자세한 충돌 스택 얻기
                //settings.CefCommandLineArgs.Add("renderer-startup-dialog");

                //settings.CefCommandLineArgs.Add("disable-application-cache", "1");
                //settings.CefCommandLineArgs.Add("disable-session-storage", "1");

                // 웹 보안 기능 비활성화
                settings.CefCommandLineArgs.Add("disable-web-security", "1");
                // 파일로부터 로드된 리소스에 대한 파일 액세스 허용
                //settings.CefCommandLineArgs.Add("allow-file-access-from-files", "1"); 

                settings.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/96.0.4664.110 Safari/537.36 /CefSharp Browser" + Cef.CefSharpVersion;

                DependencyChecker.AssertAllDependenciesPresent(settings.Locale, settings.LocalesDirPath, settings.ResourcesDirPath, settings.PackLoadingDisabled, settings.BrowserSubprocessPath);

                CefSharp.Cef.Initialize(settings);

            }
        }

        /// <summary>
        /// CefSharp에 Load된 구글, 네이버, 카카오 쿠키 정보 삭제
        /// </summary>
        /// <returns></returns>
        private async Task ClearSpecificCookies()
        {
            var cookieManager = Cef.GetGlobalCookieManager();

            // Clear Google cookies
            await cookieManager.DeleteCookiesAsync("https://accounts.google.com", null);
            await cookieManager.DeleteCookiesAsync("https://www.google.com", null);

            // Clear Naver cookies
            await cookieManager.DeleteCookiesAsync("https://nid.naver.com", null);
            await cookieManager.DeleteCookiesAsync("https://www.naver.com", null);

            // Clear Kakao cookies
            await cookieManager.DeleteCookiesAsync("https://accounts.kakao.com", null);
            await cookieManager.DeleteCookiesAsync("https://www.kakao.com", null);
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            // Region에 MainContent 를 지정하고 바로 실행
            regionManager.RegisterViewWithRegion("MainModuleContent", typeof(MainContent));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Add Dialogs
            containerRegistry.RegisterDialog<LoginDialog, LoginDialogViewModel>();
            containerRegistry.RegisterDialog<JoinDialog, JoinDialogViewModel>();
            containerRegistry.RegisterDialog<IdentityVerificationDialog, IdentityVerificationDialogViewModel>();
            containerRegistry.RegisterDialog<PolicyContentDialog, PolicyContentDialogViewModel>();
            containerRegistry.RegisterDialog<GooglePlayDialog, GooglePlayDialogViewModel>();
            containerRegistry.RegisterDialog<RedirectDialog, RedirectDialogViewModel>();
        }
    }
}
