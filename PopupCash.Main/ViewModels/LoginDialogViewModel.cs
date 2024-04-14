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
        #endregion

        public override event Action<IDialogResult>? RequestClose;

        [ObservableProperty]
        private string _loginEmail;

        [ObservableProperty]
        private SecureString _loginPassword;

        public LoginDialogViewModel(IDialogService dialogService, IEventAggregator eventAggregator, IMapper mapper,
            ILoginService loginService, IUserService userService,
            IAuthorizationDataService authorizationService, ILogger<LoginDialogViewModel> loggor) : base(loggor)
        {
            _dialogService = dialogService;
            _eventAggregator = eventAggregator;

            _mapper = mapper;

            _loginService = loginService;
            _userService = userService;
            _authorizationService = authorizationService;

            Title = "Login";

            _loginEmail = string.Empty;
            _loginPassword = new SecureString();

            WindowWidth = 400;
            WindowHeight = 352;

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
                new LoginRequest() { Key = authorization.Key, Email = LoginEmail, Password = LoginPassword.ToUnsecuredString(), Type = "0" }));

            switch (loginResponse.Result)
            {
                case 0:
                    {
                        var auth = _mapper.Map<Authorization>(loginResponse);

                        var command = new RequestUserCommand(_mapper, _userService);

                        await IsBusyFor(async () =>
                        {
                            if (await command.ExecuteAsync(auth.AccessToken) is not CurrentUser currentUser) return;

                            authorization.AccessToken = auth.AccessToken;
                            authorization.Key = currentUser.Key;
                            _authorizationService.UpdateAuthorization(authorization);

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
            _dialogService.ShowDialog("RedirectDialog", new DialogParameters(),
                new Action<IDialogResult>(async (result) =>
                {
                    if (result.Result != ButtonResult.OK) { return; }

                    if (!result.Parameters.TryGetValue("Email", out string Email)) throw new Exception("카카오 이메일 정보가 없습니다.");


                    if (_authorizationService.SelectLastestAuthorization() is not Authorization authorization) throw new Exception("액세스 토큰 값이 없습니다.");

                    // SNS 로그인 시 이메일, 유형 으로 서버에서 중복 여부 확인 후 실패할 경우 회원가입 화면으로 이동
                    var loginResponse = await IsBusyFor(() => _loginService.LoginAsync(
                        new LoginRequest() { Key = authorization.Key, Email = Email, Type = "3" }));

                    string msg;

                    switch (loginResponse.Result)
                    {
                        case 0:
                            {
                                var auth = _mapper.Map<Authorization>(loginResponse);

                                var command = new RequestUserCommand(_mapper, _userService);

                                await IsBusyFor(async () =>
                                {
                                    if (await command.ExecuteAsync(auth.AccessToken) is not CurrentUser currentUser) return;

                                    authorization.Type = ConstantString.KakaoName;
                                    authorization.Key = currentUser.Key;
                                    authorization.AccessToken = auth.AccessToken;
                                    _authorizationService.UpdateAuthorization(authorization);

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
                                /// Join 화면으로 이동하되, 이메일 값은 입력된 상태로 이동

                                return;
                            }
                        default:
                            msg = $"{loginResponse.Msg}";
                            break;
                    }
                    throw new Exception(msg);
                })
            );
        }
    }
}
