using CefSharp;
using PopupCash.Account.Models.Login;
using PopupCash.Account.Models.Login.Impl;
using PopupCash.Account.Models.Users;
using PopupCash.Account.Models.Users.Impl;
using PopupCash.Contents.ViewModels;
using PopupCash.Contents.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace PopupCash.Contents.Models.Modules
{
    public class CashListModuleContent : IModule
    {
        public CashListModuleContent()
        {
            if (!CefSharp.Cef.IsInitialized)
            {
                var settings = new CefSharp.Wpf.CefSettings()
                {
                    //LogSeverity = LogSeverity.Disable,
                    Locale = "ko-KR", // 예: 한국어(Korea)로 설정 

                    LocalesDirPath = System.IO.Path.Combine(Environment.CurrentDirectory, "locales"),
                    ResourcesDirPath = Environment.CurrentDirectory,

                    //By default CefSharp will use an in-memory cache, you need to specify a Cache Folder to persist data
                    CachePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CefSharp\\Cache"),
                };
                // 더 자세한 충돌 스택 얻기
                //settings.CefCommandLineArgs.Add("renderer-startup-dialog");

                //settings.CefCommandLineArgs.Add("disable-application-cache", "1");
                //settings.CefCommandLineArgs.Add("disable-session-storage", "1");

                // 웹 보안 기능 비활성화
                settings.CefCommandLineArgs.Add("disable-web-security", "1");
                // 파일로부터 로드된 리소스에 대한 파일 액세스 허용
                //settings.CefCommandLineArgs.Add("allow-file-access-from-files", "1"); 

                DependencyChecker.AssertAllDependenciesPresent(settings.Locale, settings.LocalesDirPath, settings.ResourcesDirPath, settings.PackLoadingDisabled, settings.BrowserSubprocessPath);

                CefSharp.Cef.Initialize(settings);
                //Cef.GetGlobalCookieManager().DeleteCookies("", "");
            }
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<ILoginService, LoginService>();
            containerRegistry.RegisterSingleton<IUserService, UserService>();

            containerRegistry.RegisterDialog<ContentBrowserDialog, ContentBrowserDialogViewModel>();
            containerRegistry.RegisterDialog<DataCollectionInstructionsDialog, DataCollectionInstructionsDialogViewModel>();
            containerRegistry.RegisterDialog<AccumulateBrowserDialog, AccumulateBrowserDialogVIewModel>();
            containerRegistry.RegisterDialog<AccumulateHistoryDialog, AccumulateHistoryDialogViewModel>();

        }
    }
}
