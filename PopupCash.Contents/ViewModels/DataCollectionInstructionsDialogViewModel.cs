using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using PopupCash.Common.ViewModels;
using PopupCash.Contents.Models.DataCollectionItems;
using PopupCash.Database.Models.Services;
using PopupCash.Database.Models.Users;
using Prism.Services.Dialogs;

namespace PopupCash.Contents.ViewModels
{
    /// <summary>
    /// 정보 수집 윈도우 ViewModel
    /// </summary>
    public partial class DataCollectionInstructionsDialogViewModel : DialogViewModelBase
    {
        private readonly IAuthorizationDataService _authorizationService;

        [ObservableProperty]
        private ObservableCollection<DataCollectionDataRow> _dataCollections;

        public override event Action<IDialogResult>? RequestClose;

        public DataCollectionInstructionsDialogViewModel(IAuthorizationDataService authorizationService,
            ILogger<DialogViewModelBase> loggor) : base(loggor)
        {
            Title = "Information";

            _authorizationService = authorizationService;

            // DataGrid에 표시할 데이터
            DataCollections = new ObservableCollection<DataCollectionDataRow>()
            {
                new DataCollectionDataRow("ID", "중복참여 필터를 위해 필요"),
                new DataCollectionDataRow("PC IP", "중복참여 필터를 위해 필요"),
                new DataCollectionDataRow("시스템 정보", "고객 CS 응대를 위해 필요"),
            };
        }
        #region Winodw Position Methods

        public override Task LoadedDialog(RoutedEventArgs args)
        {
            return Task.CompletedTask;
        }
        public override void MouseLeftButtonDownDialog(MouseButtonEventArgs args)
        {
            return;
        }
        #endregion

        public override bool CanCloseDialog()
        {
            return true;
        }

        public override void OnDialogClosed()
        {
            Cancel();
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {

        }
        [RelayCommand]
        public void Cancel()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
        }

        [RelayCommand]
        public void Ok()
        {
            if (_authorizationService.SelectLastestAuthorization() is not Authorization authorization) throw new Exception("액세스 토큰 값이 없습니다.");
            authorization.Policy = true;
            var isUpdate = _authorizationService.InsertOrUpdateAuthorization(authorization);
            logger.LogDebug($"Update Authorize is {isUpdate}");

            RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
        }
    }
}
