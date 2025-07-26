using System.Windows;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using Prism.Services.Dialogs;

namespace PopupCash.Common.Models.Dialogs.Parameters
{
    [ObservableObject]
    public partial class
        InformationDialogParameter : DialogParameters
    {
        [ObservableProperty]
        private string _text;

        [ObservableProperty]
        public Brush _textColor;

        [ObservableProperty]
        public FontWeight _textFontWeight;


        [ObservableProperty]
        public string _subText;

        [ObservableProperty]
        public Brush _subTextColor;

        [ObservableProperty]
        public FontWeight _subTextFontWeight;

        [ObservableProperty]
        public string _confirmButtonText;

        [ObservableProperty]
        public bool _useSubText;

        [ObservableProperty]
        public bool _isOkCancel;

        [ObservableProperty]
        public int _autoExit;

        [ObservableProperty]
        private string? _owendWindowTitle;

        // 매개변수로 받는 속성들을 초기화하는 생성자
        public InformationDialogParameter(string text, Brush textColor, FontWeight textFontWeight,
                                          string subText, Brush subTextColor, FontWeight subTextFontWeight,
                                          string confirmButtonText,
                                          bool useSubText, bool isOkCancel,
                                          int autoExit = 0,
                                          string? owendWindowTitle = null)
        {
            Text = text;
            TextColor = textColor;
            TextFontWeight = textFontWeight;
            SubText = subText;
            SubTextColor = subTextColor;
            SubTextFontWeight = subTextFontWeight;
            ConfirmButtonText = confirmButtonText;
            UseSubText = useSubText;
            IsOkCancel = isOkCancel;
            _autoExit = autoExit;
            _owendWindowTitle = owendWindowTitle;
        }
    }
}