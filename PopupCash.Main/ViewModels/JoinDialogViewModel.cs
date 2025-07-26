using System.Text;
using AutoMapper;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.Extensions.Logging;
using PopupCash.Account.Models.Login;
using PopupCash.Account.Models.Login.Impl;
using PopupCash.Account.Models.Users;
using PopupCash.Common.ViewModels;
using PopupCash.Controls.Buttons;
using PopupCash.Core.Extensions;
using PopupCash.Core.Models.Constants;
using PopupCash.Core.Models.Parameters;
using PopupCash.Database.Models.Services;
using PopupCash.Database.Models.Users;
using PopupCash.Main.Models.Commands.Impl;
using PopupCash.Main.Models.Events;
using PopupCash.Main.Models.Joins;
using PopupCash.Main.Models.Users;
using PopupCash.Main.Models.Verifications;

using Prism.Events;
using Prism.Services.Dialogs;

namespace PopupCash.Main.ViewModels
{
    public partial class JoinDialogViewModel : DialogViewModelBase
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
        private JoinInfo _joinInfo;

        public JoinDialogViewModel(IDialogService dialogService, IEventAggregator eventAggregator, IMapper mapper,
            ILoginService loginService, IUserService userService, IAuthorizationDataService authorizationService,
            ILogger<JoinDialogViewModel> loggor, ILoggerFactory loggerFactory) : base(loggor)
        {
            _dialogService = dialogService;
            _eventAggregator = eventAggregator;

            _mapper = mapper;

            _loginService = loginService;
            _userService = userService;

            _authorizationService = authorizationService;

            _loggerFactory = loggerFactory;

            Title = "회원 가입";

            // 윈도우 크기
            WindowWidth = 400;
            //WindowHeight = 858;

            JoinInfo = new JoinInfo();

            _eventAggregator.GetEvent<IdentityVerificationEvent>().Subscribe(OnReceivedIdentityVerification, ThreadOption.UIThread);
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
            if (parameters.TryGetValue("loginType", out string loginType))
            {
                JoinInfo.LoginType = loginType;
            }
            if (parameters.TryGetValue("email", out string email))
            {
                JoinInfo.Email = email;
            }

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
        public void CloseWindow()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel, new DialogParameters()
            {
            }));
        }

        //[RelayCommand(CanExecute = "CanJoinExcute")]
        [RelayCommand]
        public async Task Join()
        {
            //if (!JoinInfo.IsIdentityVerification) throw new Exception("본인 인증이 완료되지 않았습니다.");
            //if (!JoinInfo.ValidateEmail()) throw new Exception($"이메일 형식이 다릅니다.");
            //if (_loginType.Equals(ConstantString.AppType) && !JoinInfo.ValidatePassword()) throw new Exception($"비밀 번호가 일치하지 않습니다.");
            //if (!JoinInfo.ValidateChecked()) throw new Exception($"동의 사항이 모두 체크되어야 합니다.");

            if (_authorizationService.SelectLastestAuthorization() is not Authorization authorization) throw new Exception("액세스 토큰 값이 없습니다.");

            var joinRequest = _mapper.Map<JoinRequest>(JoinInfo);
            joinRequest.Key = authorization.Key;
            var joinResponse = await IsBusyFor(() => _loginService.JoinAsync(joinRequest));

            if (joinResponse.Result == 0)
            {
                var auth = _mapper.Map<Authorization>(joinResponse);

                var command = new RequestUserCommand(_mapper, _userService, _loggerFactory.CreateLogger<RequestUserCommand>());

                await IsBusyFor(async () =>
                {
                    if (await command.ExecuteAsync(auth.AccessToken!) is not CurrentUser currentUser) return;

                    authorization.Type = JoinInfo.LoginType;
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
            else
            {
                StringBuilder sb = new StringBuilder();
                switch (joinResponse.Result)
                {
                    case 5:
                        {
                            sb.Append(string.Format($"{ConstantString.AppName}에 "));
                        }
                        break;
                    case 7:
                        {
                            if (!string.IsNullOrEmpty(joinResponse.Type))
                                sb.Append(string.Format($"{joinResponse.Type.GetJoinType()}에 "));
                        }
                        break;
                }

                sb.Append(joinResponse.Msg);

                logger.LogDebug($"Result: {joinResponse.Result}");
                throw new Exception(sb.ToString());
            }
        }

        [RelayCommand]
        public void GotoIdentityVerificationDialog()
        {
            _dialogService.ShowDialog("IdentityVerificationDialog", new DialogParameters(),
                new Action<IDialogResult>((result) =>
                {
                    if (result.Result != ButtonResult.OK) { return; }
                })
            );
        }


        [RelayCommand]
        public async Task GotoPolicyDialog()
        {
            var policyResponse = await IsBusyFor(() => _loginService.GetPolicyAsync());

            if (policyResponse.Result == 0)
            {
                if (!string.IsNullOrEmpty(policyResponse.Content))
                {
                    _dialogService.Show("PolicyContentDialog", new StringTextParameter() { Text = policyResponse.Content },
                        new Action<IDialogResult>((result) =>
                        {
                        })
                    );
                }
            }
        }

        [RelayCommand]
        public async Task GotoPrivacyDialog()
        {
            var privacyResponse = await IsBusyFor(() => _loginService.GetPrivacyAsync());

            if (privacyResponse.Result == 0)
            {
                if (!string.IsNullOrEmpty(privacyResponse.Content))
                {
                    _dialogService.Show("PolicyContentDialog", new StringTextParameter() { Text = privacyResponse.Content },
                        new Action<IDialogResult>((result) =>
                        {
                        })
                    );
                }
            }
        }

        [RelayCommand]
        public void CheckPolicy(ImageButton imageButton)
        {
            imageButton.IsImageToolge = !imageButton.IsImageToolge;

            JoinInfo.IsCheckedPolicy = imageButton.IsImageToolge;
        }

        [RelayCommand]
        public void CheckPrivacy(ImageButton imageButton)
        {
            imageButton.IsImageToolge = !imageButton.IsImageToolge;

            JoinInfo.IsCheckedPrivacy = imageButton.IsImageToolge;
        }

        [RelayCommand]
        public void CheckAge(ImageButton imageButton)
        {
            imageButton.IsImageToolge = !imageButton.IsImageToolge;

            JoinInfo.IsCheckedAge = imageButton.IsImageToolge;
        }

        private void OnReceivedIdentityVerification(ResponseIdentityVerification? verification)
        {
            if (verification is null) { return; }

            var identityVerificationResponse = _mapper.Map<IdentityVerificationResponse>(verification);
            var joinInfo = _mapper.Map<JoinInfo>(identityVerificationResponse);

            JoinInfo.Name = joinInfo.Name;
            JoinInfo.Phone = joinInfo.Phone;
            JoinInfo.Birth = joinInfo.Birth;
            JoinInfo.Sex = joinInfo.Sex;

            // 본인 인증 성공
            JoinInfo.IsIdentityVerification = true;
        }
    }
}
