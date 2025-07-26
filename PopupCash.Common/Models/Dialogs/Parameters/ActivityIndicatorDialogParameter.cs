using System.Windows;
using System.Windows.Media;

using CommunityToolkit.Mvvm.ComponentModel;

using Prism.Services.Dialogs;

namespace PopupCash.Common.Models.Dialogs.Parameters
{
    [ObservableObject]
    public partial class ActivityIndicatorDialogParameter : DialogParameters
    {
        [ObservableProperty]
        private string _owendWindowTitle;

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
        public bool _useSubText;


        // 매개변수로 받는 속성들을 초기화하는 생성자
        public ActivityIndicatorDialogParameter(string owendWindowTitle, string text, Brush textColor, FontWeight textFontWeight,
                                          string subText, Brush subTextColor, FontWeight subTextFontWeight,
                                          bool useSubText)
        {
            OwendWindowTitle = owendWindowTitle;
            Text = text;
            TextColor = textColor;
            TextFontWeight = textFontWeight;
            SubText = subText;
            SubTextColor = subTextColor;
            SubTextFontWeight = subTextFontWeight;
            UseSubText = useSubText;
        }
    }
}