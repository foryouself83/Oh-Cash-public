using System.Diagnostics;
using System.Windows.Navigation;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using PopupCash.Common.ViewModels;
using PopupCash.Main.Models.OpenSources;
using Prism.Services.Dialogs;

namespace PopupCash.Main.ViewModels
{

    public partial class OpenSourceLicenseDialogViewModel : DialogViewModelBase
    {
        public override event Action<IDialogResult>? RequestClose;

        [ObservableProperty]
        List<OpenSourceInfo>? _openSources;

        public OpenSourceLicenseDialogViewModel(ILogger<OpenSourceLicenseDialogViewModel> loggor) : base(loggor)
        {
            Title = "오픈소스 정보";

            // 윈도우 크기
            WindowWidth = 452;
            WindowHeight = 400;
            OpenSources = new List<OpenSourceInfo>()
            {
                new OpenSourceInfo("AutoMapper", "https://github.com/AutoMapper/AutoMapper?tab=MIT-1-ov-file#readme"),
                new OpenSourceInfo("BusyIndicator", "https://github.com/Moh3nGolshani/BusyIndicator/tree/master?tab=MIT-1-ov-file#readme"),
                new OpenSourceInfo("CefSharp.Wpf.NETCore", "https://github.com/cefsharp/CefSharp?tab=License-1-ov-file#readme"),
                new OpenSourceInfo("chromiumembeddedframework.runtime.win-x64", "https://www.nuget.org/packages/chromiumembeddedframework.runtime.win-x64/123.0.6/license"),
                new OpenSourceInfo("chromiumembeddedframework.runtime.win-x86", "https://www.nuget.org/packages/chromiumembeddedframework.runtime.win-x86/123.0.6/license"),
                new OpenSourceInfo("CommunityToolkit.Mvvm", "https://github.com/CommunityToolkit/dotnet?tab=License-1-ov-file#readme"),
                new OpenSourceInfo("Dapper", "https://github.com/DapperLib/Dapper?tab=License-1-ov-file#readme"),
                new OpenSourceInfo("Dapper.FluentMap", "https://github.com/henkmollema/Dapper-FluentMap?tab=MIT-1-ov-file#readme"),
                new OpenSourceInfo("Hardcodet.NotifyIcon.Wpf", "https://www.nuget.org/packages/Hardcodet.NotifyIcon.Wpf/1.1.0/license"),
                new OpenSourceInfo("Microsoft.Data.Sqlite.Core", "https://github.com/dotnet/efcore?tab=MIT-1-ov-file#readme"),
                new OpenSourceInfo("Microsoft.Extensions.Configuration.Json", "https://github.com/dotnet/runtime?tab=MIT-1-ov-file#readme"),
                new OpenSourceInfo("Microsoft.Extensions.DependencyInjection", "https://github.com/dotnet/runtime?tab=MIT-1-ov-file#readme"),
                new OpenSourceInfo("Microsoft.Extensions.Http", "https://github.com/dotnet/runtime?tab=MIT-1-ov-file#readme"),
                new OpenSourceInfo("Microsoft.Xaml.Behaviors.Wpf", "https://github.com/Microsoft/XamlBehaviorsWpf?tab=MIT-1-ov-file#readme"),
                new OpenSourceInfo("Newtonsoft.Json", "https://github.com/JamesNK/Newtonsoft.Json?tab=MIT-1-ov-file#readme"),
                new OpenSourceInfo("NLog", "https://github.com/NLog/NLog?tab=BSD-3-Clause-1-ov-file#readme"),
                new OpenSourceInfo("NLog.Extensions.Logging", "https://github.com/NLog/NLog.Extensions.Logging?tab=BSD-2-Clause-1-ov-file#readme"),
                new OpenSourceInfo("Prism.Unity", "https://www.nuget.org/packages/Prism.Unity/8.1.97/license"),
                new OpenSourceInfo("Prism.Unity.Extensions", "https://github.com/dansiegel/Prism.Container.Extensions?tab=MIT-1-ov-file#readme"),
                new OpenSourceInfo("Prism.Wpf", "https://www.nuget.org/packages/Prism.Wpf/8.1.97/license"),
                new OpenSourceInfo("SQLitePCLRaw.bundle_e_sqlcipher", "https://github.com/ericsink/SQLitePCL.raw?tab=Apache-2.0-1-ov-file#readme"),
            };

        }

        #region IDialogAware Methods
        public override bool CanCloseDialog()
        {
            return true;
        }

        public override void OnDialogClosed()
        {
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
        }
        [RelayCommand]
        public void Cancel()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel, new DialogParameters()
            {
            }));
        }
        #endregion

        [RelayCommand]
        public void Confirm()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK, new DialogParameters()
            {
            }));
        }
        [RelayCommand]
        public void RequestNavigate(RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo("cmd", $"/c start {e.Uri.AbsoluteUri}") { CreateNoWindow = true });
            e.Handled = true;
        }
    }
}
