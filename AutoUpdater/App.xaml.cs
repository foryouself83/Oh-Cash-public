using System.Windows;
using System.Windows.Threading;

namespace AutoUpdater
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public string[]? Args { get; set; }

        public App()
        {
            Args = null;
            Dispatcher.UnhandledExceptionFilter += Dispatcher_UnhandledExceptionFilter;
            Dispatcher.UnhandledException += Dispatcher_UnhandledException;

            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            // args 배열에는 실행할 때 전달된 명령줄 인수가 포함됩니다.
            Args = e.Args;

            base.OnStartup(e);
        }

        private void TaskScheduler_UnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
        {
            ShowMessageBox(e.Exception.Message);
            Environment.Exit(0);
        }

        private void Dispatcher_UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            ShowMessageBox(e.Exception.Message);
            Environment.Exit(0);
        }

        private void Dispatcher_UnhandledExceptionFilter(object sender, DispatcherUnhandledExceptionFilterEventArgs e)
        {
            // true를 설정하면 응용 프로그램이 비정상 종료되지 않으나 false를 설정하면 응용 프로그램이 비정상 종료된다.
            e.RequestCatch = true;
        }

        private void ShowMessageBox(string msg)
        {
            MessageBox.Show(msg);
        }
    }

}
