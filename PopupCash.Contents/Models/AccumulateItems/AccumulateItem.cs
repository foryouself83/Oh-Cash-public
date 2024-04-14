using CommunityToolkit.Mvvm.ComponentModel;

namespace PopupCash.Contents.Models.AccumulateItems
{
    public partial class AccumulateItem : ObservableObject
    {
        [ObservableProperty]
        public string _title;

        [ObservableProperty]
        public string _description;

        [ObservableProperty]
        public string _currentValue;

        [ObservableProperty]
        public string _maxValue;

        public AccumulateItem()
        {
            _title = string.Empty;
            _description = string.Empty;
            _currentValue = string.Empty;
            _maxValue = string.Empty;
        }
    }
}
