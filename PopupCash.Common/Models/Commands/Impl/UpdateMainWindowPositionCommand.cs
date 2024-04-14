using Microsoft.Win32;
using PopupCash.Core.Models.Commands;
using PopupCash.Database.Models.Locations;

namespace PopupCash.Common.Models.Commands.Impl
{
    public class UpdateMainWindowPositionCommand : ICommandVoid<WindowPosition>
    {
        private readonly string _id;
        private readonly string _keyName;
        private readonly string _valueLeftName;
        private readonly string _valueTopName;

        private bool _canExecute = true;
        public event EventHandler? CanExecuteChanged;

        public UpdateMainWindowPositionCommand(string id)
        {
            _id = id;
            _keyName = $@"Software\PopupCash\{id}";
            _valueLeftName = "Left";
            _valueTopName = "Top";
        }

        public bool CanExecute(object? parameter)
        {
            return _canExecute;
        }

        public void Execute(WindowPosition position)
        {
            WriteRegistryValue(_keyName, _valueLeftName, position.Left);
            WriteRegistryValue(_keyName, _valueTopName, position.Top);
        }

        public void RaiseCanExecuteChanged()
        {
            // CanExecuteChanged 이벤트가 null이 아니면 호출
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        // 레지스트리에 값을 쓰는 메서드
        private void WriteRegistryValue(string keyName, string valueName, object value)
        {
            try
            {
                using (var key = Registry.CurrentUser.CreateSubKey(keyName))
                {
                    key.SetValue(valueName, value);
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
