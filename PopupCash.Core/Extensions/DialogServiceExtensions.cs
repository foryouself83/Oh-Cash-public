using Prism.Services.Dialogs;

namespace PopupCash.Core.Extensions
{
    public static class DialogServiceExtensions
    {
        private static List<string> _isShowDialogs = new List<string>();

        /// <summary>
        /// Dialog를 한번만 보여주기 위한 메서드
        /// </summary>
        /// <param name="dialogService"></param>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        /// <param name="callback"></param>
        public static void ShowOnce(this IDialogService dialogService, string name, IDialogParameters parameters, Action<IDialogResult>? callback)
        {

            if (!_isShowDialogs.Contains(name))
            {
                _isShowDialogs.Add(name);
                dialogService.Show(name, parameters, result =>
                {
                    _isShowDialogs.Remove(name);
                    callback?.Invoke(result);
                });
            }
        }

        public static int ShowOnceCount(this IDialogService dialogService)
        {
            return _isShowDialogs.Count;
        }
    }
}
