using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PopupCash.Contents.Models.DataCollectionItems;
using Prism.Services.Dialogs;

namespace PopupCash.Contents.ViewModels
{
    /// <summary>
    /// 정보 수집 윈도우 ViewModel
    /// </summary>
    public partial class DataCollectionInstructionsDialogViewModel : ObservableObject, IDialogAware
    {
        [ObservableProperty]
        private ObservableCollection<DataCollectionDataRow> _dataCollections;

        public string Title => "Information";

        public event Action<IDialogResult>? RequestClose;

        public DataCollectionInstructionsDialogViewModel()
        {
            // DataGrid에 표시할 데이터
            DataCollections = new ObservableCollection<DataCollectionDataRow>()
            {
                new DataCollectionDataRow("ID", "중복참여 필터를 위해 필요"),
                new DataCollectionDataRow("PC IP", "중복참여 필터를 위해 필요"),
                new DataCollectionDataRow("시스템 정보", "고객 CS 응대를 위해 필요"),
            };
        }


        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            Cancel();
        }

        public void OnDialogOpened(IDialogParameters parameters)
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
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
        }
    }
}
