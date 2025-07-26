using System.Diagnostics;
using AutoMapper;
using Microsoft.Extensions.Logging;
using PopupCash.Account.Models.Helpers;
using PopupCash.Account.Models.Users;
using PopupCash.Account.Models.Users.Impl;
using PopupCash.Core.Models.Commands;
using PopupCash.Main.Models.Users;

namespace PopupCash.Main.Models.Commands.Impl
{
    internal class RequestUserCommand : ICommandAsync<CurrentUser>
    {
        private readonly IMapper _mapper;

        private readonly IUserService _userService;

        private readonly ILogger _logger;


        private bool _canExecute = true;

        public event EventHandler? CanExecuteChanged;

        public RequestUserCommand(IMapper mapper, IUserService userService, ILogger<RequestUserCommand> logger)
        {
            _mapper = mapper;

            _userService = userService;

            _logger = logger;
        }


        public bool CanExecute(object? parameter)
        {
            return _canExecute;
        }

        public async Task<CurrentUser?> ExecuteAsync(object? parameter)
        {
            if (parameter is not string accessToken) throw new Exception("잘못된 파라메터 입니다.");

            if (await _userService.GetCurrentUser(accessToken) is not UserResponse loginResponse) throw new Exception($"서버에서 정보를 가져오는 데 실패하였습니다.");

            if (loginResponse.Result == 0)
            {
                CurrentUser currentUser = _mapper.Map<CurrentUser>(loginResponse);
                Debug.Assert(!string.IsNullOrEmpty(currentUser.Name), "Name is empty.");

                currentUser.Mac ??= MacAddressHelper.GetMacAddress();
                currentUser.AccessToken = accessToken;

                Debug.Assert(!string.IsNullOrEmpty(currentUser.Mac), "Mac is empty.");

                return currentUser;
            }
            else
            {
                _logger.LogDebug($"Result: {loginResponse.Result}");

                if (!string.IsNullOrEmpty(loginResponse.msg))
                    throw new Exception(loginResponse.msg);
                else
                    throw new Exception("사용자 정보를 받는데 실패하였습니다.");
            }
        }
        public void RaiseCanExecuteChanged()
        {
            // CanExecuteChanged 이벤트가 null이 아니면 호출
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
