using AutoMapper;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.Extensions.Logging;

using PopupCash.Account.Models.Login;
using PopupCash.Account.Models.Login.Impl;
using PopupCash.Account.Models.Users;
using PopupCash.Common.Models.Events;
using PopupCash.Common.ViewModels;
using PopupCash.Core.Extensions;
using PopupCash.Core.Models.Constants;
using PopupCash.Core.Models.Parameters;
using PopupCash.Database.Models.Services;
using PopupCash.Database.Models.Users;
using PopupCash.Main.Models.Commands.Impl;
using PopupCash.Main.Models.Users;

using Prism.Events;
using Prism.Services.Dialogs;

namespace PopupCash.Main.ViewModels
{
    /// <summary>
    /// 메인 화면 뷰 모델
    /// </summary>
    public partial class MainContentViewModel : ViewModelBase
    {
        #region Prism
        private readonly IDialogService _dialogService;
        private readonly IEventAggregator _eventAggregator;
        #endregion

        #region AutoMapper
        private readonly IMapper _mapper;
        #endregion

        #region Services
        private readonly ILoginService _loginService;
        private readonly IUserService _userService;

        private readonly IDatabaseFactory _databaseFactory;
        private readonly IAuthorizationDataService _authorizationService;
        private readonly IUserDataService _userDataService;

        private readonly ILoggerFactory _loggerFactory;
        #endregion

        [ObservableProperty]
        private CurrentUser? _user;

        [ObservableProperty]
        private bool _isMember;

        public MainContentViewModel(IDialogService dialogService, IEventAggregator eventAggregator, IMapper mapper,
            ILoginService loginService, IUserService userService,
            IDatabaseFactory databaseFactory, IAuthorizationDataService authorizationService, IUserDataService userDataService,
            ILogger<MainContentViewModel> loggor, ILoggerFactory loggerFactory) : base(loggor)
        {
            _dialogService = dialogService;
            _eventAggregator = eventAggregator;

            _mapper = mapper;

            _loginService = loginService;
            _userService = userService;

            _databaseFactory = databaseFactory;
            _authorizationService = authorizationService;
            _userDataService = userDataService;

            _loggerFactory = loggerFactory;

            IsMember = false;

            this.User = new CurrentUser();

            _eventAggregator.GetEvent<UpdateCurrentUserEvent>().Subscribe(OnReceiveUpdateCurrentUser, ThreadOption.UIThread);
        }

        private async void OnReceiveUpdateCurrentUser(bool isUpdate)
        {
            if (isUpdate)
            {
                var result = await UpdateUserFromServerAsync();
            }
        }

        [RelayCommand]
        public async Task LoadedWindow()
        {
            // 기존 로그인한 유저가 있는 경우 자동 로그인
            if (await UpdateUserFromServerAsync() == true) return;


            // TODO. API로 Database 암호 받을 경우 자동 로그인이 눈에 보이는 현상이 있음.
            var nonJoinResponse = await IsBusyFor(() => _loginService.NonJoinAsync(new NonJoinRequest()));

            if (nonJoinResponse.Result == 0)
            {
                // 비회원 정보로 업데이트
                if (string.IsNullOrEmpty(nonJoinResponse.Key) || string.IsNullOrEmpty(nonJoinResponse.Token)) throw new Exception("비회원 정보가 없습니다.");
                _authorizationService.InsertOrUpdateAuthorization(new Authorization() { Type = ConstantString.AppType, Key = nonJoinResponse.Key, AccessToken = nonJoinResponse.Token, Policy = false });
                await UpdateUserFromServerAsync();
            }
            else
            {
                logger.LogDebug($"Result: {nonJoinResponse.Result}");
                throw new Exception($"{nonJoinResponse.Msg}");
            }
        }

        [RelayCommand]
        public void GotoLoginDialog()
        {
            if (_dialogService.ShowOnceCount() > 0) throw new Exception("열려 있는 윈도우를 닫아야 로그인/회원가입이 가능합니다.");

            _dialogService.ShowDialog("LoginDialog", new DialogParameters(),
                new Action<IDialogResult>(async (result) =>
                {
                    if (result.Result != ButtonResult.OK) { return; }

                    await UpdateUserFromServerAsync();

                    //if (result.Parameters.TryGetValue("CurrentUser", out CurrentUser currentUser))
                    //{
                    //    UpdateUserInfo(currentUser);
                    //}
                })
            );
        }

        [RelayCommand]
        public void GotoAccumulate()
        {
            if (User is null) throw new Exception("사용자 정보가 없습니다.");
            if (_loginService.GetPomissionInfo() is not PomissionInfo pomissionInfo) throw new Exception("Pomission 정보가 없습니다.");

            logger.LogDebug($"MAC: {User.Mac} / Pomission Key: {User.PomissionKey}, RefreshToken: {pomissionInfo.RefreshToken}");

            var url = $"https://pomission.com/common/view/missionList?pomissionMediaId={pomissionInfo.MediaId}&pomissionRefreshToken={pomissionInfo.RefreshToken}&userAdId={User.Mac}&userUuId={User.PomissionKey}&useService=popupcashpc&deviceName={Environment.OSVersion.VersionString}";

            _dialogService.ShowOnce("AccumulateBrowserDialog", new MoveAddressParameter() { Url = url },
                new Action<IDialogResult>(async (result) =>
                {
                    await UpdateUserFromServerAsync();
                })
            );
        }

        [RelayCommand]
        public void GotoAccumulateHistory()
        {
            _dialogService.ShowOnce("AccumulateHistoryDialog", new DialogParameters(),
                new Action<IDialogResult>(async (result) =>
                {
                    await UpdateUserFromServerAsync();
                })
            );
        }

        [RelayCommand]
        public void GotoExchangeNeverPay()
        {
            _dialogService.ShowOnce("GooglePlayDialog", new DialogParameters(),
                new Action<IDialogResult>(async (result) =>
                {
                    await UpdateUserFromServerAsync();
                })
            );
        }

        private async Task<bool> UpdateUserFromServerAsync()
        {
            if (_authorizationService.SelectLastestAuthorization() is not Authorization authorization) return false;

            var command = new RequestUserCommand(_mapper, _userService, _loggerFactory.CreateLogger<RequestUserCommand>());

            await IsBusyFor(async () =>
            {
                if (await command.ExecuteAsync(authorization.AccessToken) is not CurrentUser currentUser)
                {
                    throw new Exception($"사용자 정보 업데이트 실패");
                }
                var userData = _mapper.Map<UserData>(currentUser);
                _userDataService.InsertOrUpdateAuthorization(userData);

                if (currentUser.PomissionKey is not null && !currentUser.PomissionKey.Equals(authorization.PomissionKey))
                {
                    authorization.PomissionKey = currentUser.PomissionKey!;
                    _authorizationService.InsertOrUpdateAuthorization(authorization);
                }

                UpdateUserInfo(currentUser);
            });

            return true;
        }


        private void UpdateUserInfo(CurrentUser user)
        {
            logger.LogDebug($"사용자 정보 업데이트");
            // UI 갱신
            User = user;
            IsMember = user.Flag == "1" ? true : false;
        }
    }
}
