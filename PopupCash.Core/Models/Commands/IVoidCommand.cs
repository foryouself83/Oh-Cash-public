namespace PopupCash.Core.Models.Commands
{
    public interface ICommandVoid<T>
    {

        /// <summary>
        /// 실행 가능 여부 확인 메서드
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object? parameter);
        /// <summary>
        /// 실행 메서드
        /// </summary>
        /// <param name="acessToken"></param>
        /// <returns></returns>
        public void Execute(T parameter);

        /// <summary>
        /// 실행 가능성이 변경되었음을 알려주는 메서드
        /// </summary>
        public void RaiseCanExecuteChanged();
    }
}