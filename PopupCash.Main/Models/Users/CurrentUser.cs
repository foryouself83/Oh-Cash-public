using CommunityToolkit.Mvvm.ComponentModel;

namespace PopupCash.Main.Models.Users
{
    public partial class CurrentUser : ObservableObject
    {
        [ObservableProperty]
        public string? _email;

        [ObservableProperty]
        public string? _name;

        [ObservableProperty]
        public string? _cash;

        [ObservableProperty]
        public int _missionPoint;

        [ObservableProperty]
        public string? _flag;

        /// <summary>
        /// 미션 리스트 URI 사용
        /// </summary>
        public string? Mac { get; set; }
        /// <summary>
        /// 미션 리스트 URI 사용
        /// </summary>
        public string? Key { get; set; }
    }
}
