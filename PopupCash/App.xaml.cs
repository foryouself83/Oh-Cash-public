using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using PopupCash.Account.Extensions;
using PopupCash.Account.Models.Login;
using PopupCash.Account.Models.Login.Impl;
using PopupCash.Common.Models.Dialogs.Parameters;
using PopupCash.Common.ViewModels;
using PopupCash.Common.Views;
using PopupCash.Contents.Models.Modules;
using PopupCash.Core.Models.Constants;
using PopupCash.Database.Models.Services;
using PopupCash.Database.Models.Services.Impl;
using PopupCash.Main.Models.Mappers;
using PopupCash.Main.Models.Modules;
using PopupCash.Main.ViewModels;
using PopupCash.Main.Views;
using PopupCash.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Services.Dialogs;
using Prism.Unity;

namespace PopupCash
{

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        private Mutex? mMutex = null;

        #region WIN32 API

        // 윈도우 핸들을 최상위로 올리는 Win32 API 호출을 위한 상수 및 함수 선언
        private const int SW_RESTORE = 9;

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        #endregion


        public App()
        {
        }

        protected override IContainerExtension CreateContainerExtension() => PrismContainerExtension.Current;

        protected override void OnStartup(StartupEventArgs e)
        {
            bool createNew = false;
            if (Assembly.GetEntryAssembly() is Assembly assembly &&
                assembly.GetName().Name is string projectName)
            {
                mMutex = new Mutex(true, projectName, out createNew);
            }

            if (createNew)
            {
                PrismContainerExtension.Init();

                base.OnStartup(e);
            }
            else
            {
                BringToFront();
                // 프로그램 종료
                System.Environment.Exit(0);
            }
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Prism에서 사용할 수 없는 Service 등록
            PrismContainerExtension.Current.RegisterServices((service) =>
            {
                service.AddHttpClient();
                service.AddAutoMapper(typeof(MappingProfile));
                service.AddSingleton<IDatabaseFactory, DatabaseFactory>();
                service.AccountService();
                service.AddLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                    logging.AddNLog();
                });
            });

            // Dialogs
            containerRegistry.RegisterDialog<InformationDialog, InformationDialogViewModel>();
            containerRegistry.RegisterDialog<OpenSourceLicenseDialog, OpenSourceLicenseDialogViewModel>();
            containerRegistry.RegisterDialog<ActivityIndicator, ActivityIndicatorViewModel>();


            // rest api services
            containerRegistry.RegisterSingleton<ILoginService, LoginService>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<MainModuleContent>();
            moduleCatalog.AddModule<CashListModuleContent>();
        }

        protected override void Initialize()
        {
            Dispatcher.UnhandledExceptionFilter += Dispatcher_UnhandledExceptionFilter;
            Dispatcher.UnhandledException += Dispatcher_UnhandledException;

            //AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            base.Initialize();

        }

        private void Dispatcher_UnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                NLog.LogManager.GetCurrentClassLogger().Error(e.Exception);
                var dialogService = Container.Resolve<IDialogService>();


                dialogService.ShowDialog("InformationDialog",
                new InformationDialogParameter(e.Exception.Message, ConstantColors.DefaultMessageColor, FontWeights.Normal, string.Empty, ConstantColors.DefaultMessageColor, FontWeights.Normal, "확인", false, false),
                new Action<IDialogResult>((result) =>
                {
                    if (result.Result != ButtonResult.OK) { return; }
                }));
            }
            catch
            { // 프로그램 안정성을 위해 예외를 잡아도 아무것도 하지 않는다.

            }
            finally
            {
                e.Handled = true;
            }
        }

        private void Dispatcher_UnhandledExceptionFilter(object sender, System.Windows.Threading.DispatcherUnhandledExceptionFilterEventArgs e)
        {
            // true를 설정하면 응용 프로그램이 비정상 종료되지 않으나 false를 설정하면 응용 프로그램이 비정상 종료된다.
            e.RequestCatch = true;
        }

        /// <summary>
        /// Unhandle Exception 처리
        /// Log 및 Message 필요
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Error(ex);
            }
        }
        /// <summary>
        /// Task Exception
        /// Log 및 Message 필요
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TaskScheduler_UnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
        {
            NLog.LogManager.GetCurrentClassLogger().Error(e.Exception);
            e.SetObserved(); // 예외가 처리되었음을 표시
        }

        /// <summary>
        /// 현재 프로그램과 동일한 프로세스명을 가진 프로세스를 최상위로 올린다.
        /// </summary>
        private void BringToFront()
        {
            Process currentProcess = Process.GetCurrentProcess();
            foreach (Process process in Process.GetProcessesByName(currentProcess.ProcessName))
            {
                if (process.Id != currentProcess.Id)
                {
                    IntPtr hWnd = process.MainWindowHandle;
                    if (hWnd != IntPtr.Zero)
                    {
                        ShowWindow(hWnd, SW_RESTORE);
                        SetForegroundWindow(hWnd);
                        break;
                    }
                }
            }
        }
    }
}
