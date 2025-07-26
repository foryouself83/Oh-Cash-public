using AutoMapper;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.Extensions.Logging;

using PopupCash.Account.Models.Login;
using PopupCash.Account.Models.Login.Impl;
using PopupCash.Account.Models.Users;
using PopupCash.Common.ViewModels;
using PopupCash.Controls.Buttons;
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
        #endregion

        public override event Action<IDialogResult>? RequestClose;

        /// <summary>
        /// 가입 유형(0: 힐링캐시, 1:구글, 2: 페이스북, 3: 카카오, 4: 네이버)
        /// </summary>
        private string _loginType;

        [ObservableProperty]
        private JoinInfo _joinInfo;

        public JoinDialogViewModel(IDialogService dialogService, IEventAggregator eventAggregator, IMapper mapper,
            ILoginService loginService, IUserService userService,
            IAuthorizationDataService authorizationService, ILogger<JoinDialogViewModel> loggor) : base(loggor)
        {
            _dialogService = dialogService;
            _eventAggregator = eventAggregator;

            _mapper = mapper;

            _loginService = loginService;
            _userService = userService;

            _authorizationService = authorizationService;

            Title = "회원 가입";
            _loginType = "0";

            // 윈도우 크기
            WindowWidth = 400;
            WindowHeight = 858;

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
            //  TODO. Parameter에 따라 _loginType 변경, SNS 로그인으로 출력됐을 경우 이메일 주소 자동 입력 후 Readonly 처리

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
        public bool CanJoinExcute()
        {
            //return !IsBusy;
            return !IsBusy && JoinInfo.ValidateEmail() && JoinInfo.ValidatePassword() && JoinInfo.ValidateChecked();
        }

        //[RelayCommand(CanExecute = "CanJoinExcute")]
        [RelayCommand]
        public async Task Join()
        {
            if (!JoinInfo.IsIdentityVerification) throw new Exception("본인 인증이 완료되지 않았습니다.");
            if (!JoinInfo.ValidateEmail()) throw new Exception($"이메일 형식이 다릅니다.");
            if (!JoinInfo.ValidatePassword()) throw new Exception($"비밀 번호가 일치하지 않습니다.");
            if (!JoinInfo.ValidateChecked()) throw new Exception($"동의 사항이 모두 체크되어야 합니다.");

            if (_authorizationService.SelectLastestAuthorization() is not Authorization authorization) throw new Exception("액세스 토큰 값이 없습니다.");

            var joinRequest = _mapper.Map<JoinRequest>(JoinInfo);
            joinRequest.Key = authorization.Key;
            var JoinResponse = await IsBusyFor(() => _loginService.JoinAsync(joinRequest));

            if (JoinResponse.Result == 0)
            {
                var auth = _mapper.Map<Authorization>(JoinResponse);

                var command = new RequestUserCommand(_mapper, _userService);

                await IsBusyFor(async () =>
                {
                    if (await command.ExecuteAsync(auth.AccessToken!) is not CurrentUser currentUser) return;

                    authorization.Type = _loginType;
                    authorization.Key = currentUser.Key;
                    authorization.AccessToken = auth.AccessToken;
                    _authorizationService.UpdateAuthorization(authorization);

                    RequestClose?.Invoke(new DialogResult(ButtonResult.OK, new DialogParameters()
                    {
                            { nameof(CurrentUser), currentUser }
                    }));
                });
            }
            else
            {
                throw new Exception(JoinResponse.Msg);
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
