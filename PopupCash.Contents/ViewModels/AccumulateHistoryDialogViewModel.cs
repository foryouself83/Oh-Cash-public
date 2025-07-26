using System.Windows;

using AutoMapper;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.Extensions.Logging;

using PopupCash.Account.Models.Cashs;
using PopupCash.Common.ViewModels;
using PopupCash.Contents.Models.Cashs;
using PopupCash.Contents.Models.Commands;
using PopupCash.Database.Models.Services;
using PopupCash.Database.Models.Users;

using Prism.Services.Dialogs;

namespace PopupCash.Contents.ViewModels
{
    public partial class AccumulateHistoryDialogViewModel : DialogViewModelBase
    {
        private readonly IMapper _mapper;

        #region Services
        private readonly ICashService _cashService;
        private readonly IAuthorizationDataService _authorizationDataService;

        private readonly ILoggerFactory _loggerFactory;
        #endregion

        [ObservableProperty]
        private CashHistory? _cashHistory;

        public override event Action<IDialogResult>? RequestClose;

        public AccumulateHistoryDialogViewModel(IMapper mapper, ICashService cashService, IAuthorizationDataService authorizationDataService,
            ILogger<AccumulateHistoryDialogViewModel> loggor, ILoggerFactory loggerFactory) : base(loggor)
        {
            _mapper = mapper;

            _cashService = cashService;
            _authorizationDataService = authorizationDataService;

            _loggerFactory = loggerFactory;

            Title = "적립내역";
        }

        public async override Task LoadedDialog(RoutedEventArgs args)
        {
            if (_authorizationDataService.SelectLastestAuthorization() is not Authorization authorization) throw new Exception("액세스 토큰 값이 없습니다.");

            var command = new RequestCashSaveHistoryCommand(_mapper, _cashService, _loggerFactory.CreateLogger<RequestCashSaveHistoryCommand>());

            if (await command.ExecuteAsync(authorization.AccessToken) is not CashHistory cashHistory) return;

            CashHistory = cashHistory;
        }


        #region IDialogAware Methods

        public override bool CanCloseDialog()
        {
            return true;
        }

        public override void OnDialogClosed()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK, new DialogParameters()
            {
            }));
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
    }
}
