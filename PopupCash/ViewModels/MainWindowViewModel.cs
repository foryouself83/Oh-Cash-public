using System.Windows;
using System.Windows.Forms;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using PopupCash.Account.Models.Login;
using PopupCash.Common.Models;
using PopupCash.Common.Models.Commands.Impl;
using PopupCash.Common.ViewModels;
using PopupCash.Database.Models.Locations;
using PopupCash.Database.Models.Services;
using Prism.Services.Dialogs;

namespace PopupCash.ViewModels
{
    public partial class MainWindowViewModel : WindowViewModelBase
    {
        private readonly IDialogService _dialogService;
        private readonly ILoginService _loginService;
        private readonly IDatabaseFactory _databaseFactory;


        public MainWindowViewModel(IDialogService dialogService, ILoginService loginService, IDatabaseFactory databaseFactory, ILogger<MainWindowViewModel> loggor) : base(loggor)
        {
            _dialogService = dialogService;
            _loginService = loginService;
            _databaseFactory = databaseFactory;

            Title = "오테크";

            WindowWidth = 452;
            WindowHeight = 194;

            Initialize();
        }

        [RelayCommand]
        public virtual void LoadedWindow(Window window)
        {
        }
        private void Initialize()
        {
            var command = new ReadMainWindowPositionCommand(windowId, WindowWidth, WindowHeight);
            if (command.Execute(null) is not WindowPosition windowPosition) return;
            var screens = Screen.AllScreens;


            if (screens.All(x => !x.Bounds.Contains((int)windowPosition.Left, (int)windowPosition.Top)))
            { // 스크린 범위를 벗어났을 경우
                var pos = SystemParameterHelper.GetScreenCenterPoint(WindowWidth, WindowHeight);

                WindowLeft = pos.X;
                WindowTop = pos.Y;

                return;
            }

            // 데이터 베이스에 위치 정보가 있는 경우 위치 설정
            WindowLeft = windowPosition.Left;
            WindowTop = windowPosition.Top;
        }

        /// <summary>
        /// 윈도우가 닫힐 때 윈도우 위치 정보를 저장
        /// </summary>
        /// <param name="window"></param>
        [RelayCommand]
        public void ClosedWindow(Window window)
        {
            var command = new UpdateMainWindowPositionCommand(windowId);
            command.Execute(new WindowPosition(windowId, window.Left, window.Top));
        }

        public void ShowOpenSourceDialog()
        {
            _dialogService.Show("OpenSourceLicenseDialog");
        }
    }
}
