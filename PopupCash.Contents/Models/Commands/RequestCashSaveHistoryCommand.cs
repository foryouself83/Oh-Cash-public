using AutoMapper;
using PopupCash.Account.Models.Cashs;
using PopupCash.Account.Models.Users.Impl;
using PopupCash.Contents.Models.Cashs;
using PopupCash.Core.Models.Commands;

namespace PopupCash.Contents.Models.Commands
{
    internal class RequestCashSaveHistoryCommand : ICommandAsync<CashHistory>
    {
        private readonly ICashService _cashService;

        private readonly IMapper _mapper;

        private bool _canExecute = true;

        public event EventHandler? CanExecuteChanged;

        public RequestCashSaveHistoryCommand(IMapper mapper, ICashService cashService)
        {
            _mapper = mapper;
            _cashService = cashService;
        }


        public bool CanExecute(object? parameter)
        {
            return _canExecute;
        }

        public async Task<CashHistory?> ExecuteAsync(object? parameter)
        {
            if (parameter is not string accessToken) throw new Exception("잘못된 파라메터 입니다.");

            if (await _cashService.GetCashSaveHistory(accessToken) is not CashSaveHistoryResponse cashSaveHistoryResponse) throw new Exception($"서버에서 정보를 가져오는 데 실패하였습니다.");

            if (cashSaveHistoryResponse.Result == 0)
            {
                CashHistory cashHistory = _mapper.Map<CashHistory>(cashSaveHistoryResponse);

                return cashHistory;
            }
            else
            {
                throw new Exception("적립 내역을 받는데 실패하였습니다.");
            }
        }
        public void RaiseCanExecuteChanged()
        {
            // CanExecuteChanged 이벤트가 null이 아니면 호출
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
