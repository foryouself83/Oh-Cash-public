using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using NLog.Extensions.Logging;

using PopupCash.Account.Extensions;
using PopupCash.Account.Models.Login;
using PopupCash.Account.Models.Login.Impl;
using PopupCash.Account.Models.Users;
using PopupCash.Account.Models.Users.Impl;
using PopupCash.Common.Models.Dialogs.Parameters;
using PopupCash.Common.ViewModels;
using PopupCash.Common.Views;
using PopupCash.Contents.Models.Modules;
using PopupCash.Core.Models.Constants;
using PopupCash.Database.Models.Encryptors;
using PopupCash.Database.Models.Migrations;
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
            containerRegistry.RegisterSingleton<IUserService, UserService>();

            // database services
            containerRegistry.RegisterSingleton<IMigrationDatabaseService, MigrationDatabaseService>();
            containerRegistry.RegisterSingleton<IAuthorizationDataService, AuthorizationDataService>();
            containerRegistry.RegisterSingleton<IUserDataService, UserDataService>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<MainModuleContent>();
            moduleCatalog.AddModule<CashListModuleContent>();
        }
        protected override async void OnInitialized()
        {
            // 서버에서 암호를 받아오는 비동기 메서드 호출
            var loginService = Container.Resolve<ILoginService>();
            var databaseFactory = Container.Resolve<IDatabaseFactory>();
            var migrationDatabaseService = Container.Resolve<IMigrationDatabaseService>();
            var authorizationService = Container.Resolve<IAuthorizationDataService>();
            var userDataService = Container.Resolve<IUserDataService>();

            try
            {
                // DataBase 암호 가져오기
                var initResponse = await loginService.InitializeAsync();
                if (initResponse.Result == 0)
                {
                    loginService.SetInitData(initResponse);
                    if (string.IsNullOrEmpty(databaseFactory.Password) && !string.IsNullOrEmpty(initResponse.DatabasePassword))
                        databaseFactory.Password = initResponse.DatabasePassword;
                }
                else throw new Exception("정상적으로 초기화가 이뤄지지 않았습니다. 재실행해 주시기 바랍니다.");
                var salt = Encoding.UTF8.GetBytes($"{databaseFactory.Password}{databaseFactory.Password}{databaseFactory.Password}");
                Encryptor.GenerateKeyAndIV(salt, out var key, out var iv);
                #region Query 문 생성용
                CreateEncryporMigrationFile(key, iv);
                #endregion

                // DB Table 생성
                authorizationService.CreateTable();
                userDataService.CreateTable();
                migrationDatabaseService.CreateTable();

                // DB Migration
                MigrateDatabase(migrationDatabaseService, key, iv);
            }
            catch (Exception ex)
            {
                LogAndShowError(ex);

                Environment.Exit(0);
            }

            base.OnInitialized();
        }

        [Conditional("DEBUG")]
        private void CreateEncryporMigrationFile(byte[] key, byte[] iv)
        {
            string relativePath = @"..\..\..\..\..\Resource\";

            string relativeSourcePath = @$"{relativePath}Migration.Json";
            string sourcePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, relativeSourcePath));

            if (!File.Exists(sourcePath)) throw new Exception("암호화 소스 파일이 없습니다.");
            var plainText = File.ReadAllText(sourcePath);


            string relativeTargetPath = @$"{relativePath}Migration.Dat";
            string targetPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, relativeTargetPath));
            if (Encryptor.Encrypt(plainText, key, iv) is not string encrypt) throw new Exception("데이터 마이그레이션 암호화에 실패하였습니다.");

            if (!File.Exists(targetPath)) throw new Exception("암호화 대상 파일이 없습니다.");
            File.WriteAllText(targetPath, encrypt);
        }

        private void MigrateDatabase(IMigrationDatabaseService migrationDatabaseService, byte[] key, byte[] iv)
        {
            var lastVersion = migrationDatabaseService.SelectLastVersion();

            if (lastVersion == 0.0f)
            {
                migrationDatabaseService.InsertDatabaseVersion(new MigrationDatabase { Version = 1.0f, UpdateDate = DateTime.Now });
                return;
            }

            var migrationDataPath = Path.Combine(Environment.CurrentDirectory, @"Resource\Migration.Dat");
            var migrationData = ReadAndDeserializeMigrationData(migrationDataPath, key, iv);

            if (migrationData?.MigraionList == null)
            {
                return;
            }

            foreach (var data in migrationData.MigraionList.OrderBy(m => m.Version))
            {
                if (data.Version > lastVersion)
                {
                    try
                    {
                        if (data.Queries is not null)
                            migrationDatabaseService.ExecuteQuery(data.Queries);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"데이터베이스 마이그레이션 실패.\r\n version: {data.Version}\r\n{ex.Message}", ex);
                    }
                }
            }
        }

        private MigraionJsonDataList? ReadAndDeserializeMigrationData(string filePath, byte[] key, byte[] iv)
        {
            var readText = ReadJsonFromFile(filePath, key, iv);
            return JsonConvert.DeserializeObject<MigraionJsonDataList>(readText);
        }

        private void LogAndShowError(Exception ex)
        {
            NLog.LogManager.GetCurrentClassLogger().Error(ex);

            var dialogService = Container.Resolve<IDialogService>();
            dialogService.ShowDialog("InformationDialog",
                new InformationDialogParameter(ex.Message, ConstantColors.DefaultMessageColor, FontWeights.Normal, string.Empty, ConstantColors.DefaultMessageColor, FontWeights.Normal, "확인", false, false),
                (result) =>
                {
                    if (result.Result != ButtonResult.OK)
                    {
                        return;
                    }
                });
        }

        protected override void Initialize()
        {
            base.Initialize();

            Dispatcher.UnhandledExceptionFilter += Dispatcher_UnhandledExceptionFilter;
            Dispatcher.UnhandledException += Dispatcher_UnhandledException;

            //AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
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

        private string ReadJsonFromFile(string filePath, byte[] key, byte[] iv)
        {
            // 파일이 존재하는지 확인
            if (!File.Exists(filePath)) throw new Exception("데이터베이스 마이그레이션 복호화 파일이 존재하지 않습니다.");
            try
            {
                var cipherText = File.ReadAllText(filePath);

                //var textData = File.ReadAllText(filePath);
                return Encryptor.Decrypt(cipherText, key, iv);
            }
            catch (Exception ex)
            {
                throw new Exception($"데이터 마이그레이션 복호화 오류: {ex.Message}", ex);
            }
        }
    }
}
