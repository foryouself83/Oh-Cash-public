using System.Security;

using AutoMapper;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.Extensions.Logging;

using PopupCash.Account.Models.Login;
using PopupCash.Account.Models.Login.Impl;
using PopupCash.Account.Models.Users;
using PopupCash.Common.ViewModels;
using PopupCash.Core.Models.Constants;
using PopupCash.Database.Models.Services;
using PopupCash.Database.Models.Users;
using PopupCash.Main.Extensions;
using PopupCash.Main.Models.Commands.Impl;
using PopupCash.Main.Models.Users;

using Prism.Events;
using Prism.Services.Dialogs;

namespace PopupCash.Main.ViewModels
{
    public partial class LoginDialogViewModel : DialogViewModelBase
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
        private readonly IAuthorizationDataService _authorizationService;

        private readonly ILoggerFactory _loggerFactory;
        #endregion

        public override event Action<IDialogResult>? RequestClose;

        [ObservableProperty]
        private string _loginEmail;

        [ObservableProperty]
        private SecureString _loginPassword;

        public LoginDialogViewModel(IDialogService dialogService, IEventAggregator eventAggregator, IMapper mapper,
            ILoginService loginService, IUserService userService, IAuthorizationDataService authorizationService,
            ILogger<LoginDialogViewModel> loggor, ILoggerFactory loggerFactory) : base(loggor)
        {
            _dialogService = dialogService;
            _eventAggregator = eventAggregator;

            _mapper = mapper;

            _loginService = loginService;
            _userService = userService;
            _authorizationService = authorizationService;

            _loggerFactory = loggerFactory;

            Title = "Login";

            _loginEmail = string.Empty;
            _loginPassword = new SecureString();

            WindowWidth = 400;
            WindowHeight = 400;

        }
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
        public bool CanLoginExcute()
        {
            return !IsBusy;
        }

        [RelayCommand(CanExecute = "CanLoginExcute")]
        public async Task Login()
        {
            string msg;
            if (_authorizationService.SelectLastestAuthorization() is not Authorization authorization) throw new Exception("액세스 토큰 값이 없습니다.");

            var loginResponse = await IsBusyFor(() => _loginService.LoginAsync(
                new LoginRequest()
                {
                    Key = authorization.Key,
                    Email = LoginEmail,
                    Password = LoginPassword.ToUnsecuredString(),
                    Type = ConstantString.AppType
                }));

            switch (loginResponse.Result)
            {
                case 0:
                    {
                        var auth = _mapper.Map<Authorization>(loginResponse);

                        var command = new RequestUserCommand(_mapper, _userService, _loggerFactory.CreateLogger<RequestUserCommand>());

                        await IsBusyFor(async () =>
                        {
                            if (await command.ExecuteAsync(auth.AccessToken) is not CurrentUser currentUser) return;

                            authorization.Type = ConstantString.AppType;
                            authorization.AccessToken = auth.AccessToken;
                            authorization.PomissionKey = currentUser.PomissionKey!;
                            authorization.Policy = false;
                            var isUpdate = _authorizationService.InsertOrUpdateAuthorization(authorization);
                            logger.LogDebug($"Update Authorize is {isUpdate}");

                            RequestClose?.Invoke(new DialogResult(ButtonResult.OK, new DialogParameters()
                            {
                                { nameof(CurrentUser), currentUser }
                            }));
                        });
                    }
                    return;
                case 3:
                    {
                        msg = "이메일 주소를 다시 확인해주세요.";
                    }
                    break;
                case 8:
                    {
                        msg = "비밀번호를 다시 확인해주세요.";
                    }
                    break;
                default:
                    msg = $"{loginResponse.Msg}";
                    break;
            }

            throw new Exception(msg);
        }

        public bool CanFindIdExcute()
        {
            return !IsBusy;
        }

        [RelayCommand(CanExecute = "CanFindIdExcute")]
        public void FindId()
        {
        }

        public bool CanFindPasswordExcute()
        {
            return !IsBusy;
        }

        [RelayCommand(CanExecute = "CanFindPasswordExcute")]
        public void FindPassword()
        {
        }

        [RelayCommand]
        public void Join()
        {
            _dialogService.ShowDialog("JoinDialog", new DialogParameters(),
                new Action<IDialogResult>((result) =>
                {
                    if (result.Result != ButtonResult.OK) { return; }

                    if (result.Parameters.TryGetValue("CurrentUser", out CurrentUser currentUser))
                    {
                        RequestClose?.Invoke(new DialogResult(ButtonResult.OK, new DialogParameters()
                        {
                            { nameof(CurrentUser), currentUser }
                        }));
                    }
                })
            );
        }

        [RelayCommand]
        public void KakaoLogin()
        {
            _loginService.SetSocialService(ConstantString.KakaoName);
            _userService.SetSocialService(ConstantString.KakaoName);

            _dialogService.ShowDialog("RedirectDialog", new DialogParameters(),
                new Action<IDialogResult>(async (result) =>
                {
                    if (result.Result != ButtonResult.OK) { return; }

                    if (!result.Parameters.TryGetValue("accessToken", out string accessToken)) throw new Exception("카카오 토큰 정보가 없습니다.");

                    await SocialLogin(accessToken, ConstantString.KakaoName, ConstantString.KakaoType);
                })
            );
        }


        [RelayCommand]
        public void NaverLogin()
        {
            _loginService.SetSocialService(ConstantString.NaverName);
            _userService.SetSocialService(ConstantString.NaverName);

            _dialogService.ShowDialog("RedirectDialog", new DialogParameters(),
                new Action<IDialogResult>(async (result) =>
                {
                    if (result.Result != ButtonResult.OK) { return; }

                    if (!result.Parameters.TryGetValue("accessToken", out string accessToken)) throw new Exception("네이버 토큰 정보가 없습니다.");

                    await SocialLogin(accessToken, ConstantString.NaverName, ConstantString.NaverType);
                })
            );
        }


        [RelayCommand]
        public void GoogleLogin()
        {
            _loginService.SetSocialService(ConstantString.GoogleName);
            _userService.SetSocialService(ConstantString.GoogleName);

            _dialogService.ShowDialog("RedirectDialog", new DialogParameters(),
                new Action<IDialogResult>(async (result) =>
                {
                    if (result.Result != ButtonResult.OK) { return; }

                    if (!result.Parameters.TryGetValue("accessToken", out string accessToken)) throw new Exception("구글 토큰 정보가 없습니다.");

                    await SocialLogin(accessToken, ConstantString.GoogleName, ConstantString.GoogleType);
                })
            );
        }

        private async Task SocialLogin(string accessToken, string socialName, string socialType)
        {
            try
            {
                if (await _userService.GetSocialUserEmail(accessToken) is not string email) throw new Exception($"{socialName} e-mail not found.");
                if (_authorizationService.SelectLastestAuthorization() is not Authorization authorization) throw new Exception("Access token not found.");

                // SNS 로그인 시 이메일, 유형 으로 서버에서 중복 여부 확인 후 실패할 경우 회원가입 화면으로 이동
                var loginResponse = await IsBusyFor(() => _loginService.LoginAsync(
                    new LoginRequest() { Key = authorization.Key, Email = email, Type = socialType }));

                string msg;

                switch (loginResponse.Result)
                {
                    case 0:
                        {
                            var auth = _mapper.Map<Authorization>(loginResponse);

                            var command = new RequestUserCommand(_mapper, _userService, _loggerFactory.CreateLogger<RequestUserCommand>());

                            await IsBusyFor(async () =>
                            {
                                if (await command.ExecuteAsync(auth.AccessToken) is not CurrentUser currentUser) return;


                                authorization.Type = socialType;
                                authorization.PomissionKey = currentUser.PomissionKey!;
                                authorization.AccessToken = auth.AccessToken;
                                authorization.Policy = false;
                                var isUpdate = _authorizationService.InsertOrUpdateAuthorization(authorization);
                                logger.LogDebug($"Update Authorize is {isUpdate}");

                                RequestClose?.Invoke(new DialogResult(ButtonResult.OK, new DialogParameters()
                                        {
                                            { nameof(CurrentUser), currentUser }
                                        }));
                            });
                        }
                        return;
                    case 3:
                    case 8:
                        {
                            string loginType = socialType;
                            /// Join 화면으로 이동하되, 이메일 값은 입력된 상태로 이동
                            _dialogService.ShowDialog("JoinDialog", new DialogParameters()
                                        {
                                            { "loginType", loginType },
                                            { "email", email },
                                        },
                                new Action<IDialogResult>(async (result) =>
                                {
                                    if (result.Result != ButtonResult.OK)
                                    {
                                        logger.LogDebug($"가입 취소로 인한 {socialName} unlinked.");
                                        // 토큰 만료 및 로그아웃
                                        await _loginService.UnlinkAsync(accessToken);
                                        return;
                                    }

                                    if (result.Parameters.TryGetValue("CurrentUser", out CurrentUser currentUser))
                                    {
                                        RequestClose?.Invoke(new DialogResult(ButtonResult.OK, new DialogParameters()
                                        {
                                                    { nameof(CurrentUser), currentUser }
                                        }));
                                    }
                                })
                            );
                        }
                        return;
                    default:
                        msg = $"{loginResponse.Msg}";
                        break;
                }
                throw new Exception(msg);
            }
            catch
            {
                logger.LogDebug($"{socialName} unlinked.");
                // 로그인 도중 문제가 생겼을 경우 연결을 끊는다.
                // 토큰 만료 및 로그아웃
                await _loginService.UnlinkAsync(accessToken);
                throw;
            }
        }
    }
}
