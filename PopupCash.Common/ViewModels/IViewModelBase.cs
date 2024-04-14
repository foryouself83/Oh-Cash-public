namespace PopupCash.Common.ViewModels
{
    public interface IViewModelBase
    {
        /// <summary>
        /// 비동기 메서드를 중복없이 실행하고자 할때 사용
        /// </summary>
        public bool IsBusy { get; set; }
    }
}
