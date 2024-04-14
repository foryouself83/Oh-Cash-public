using Microsoft.Win32;
using PopupCash.Core.Models.Commands;
using PopupCash.Database.Models.Locations;

namespace PopupCash.Common.Models.Commands.Impl
{
    public class ReadMainWindowPositionCommand : ICommandSync<WindowPosition>
    {
        private readonly string _id;
        private readonly double _actualWidth;
        private readonly double _actualHeight;

        private readonly string _keyName;
        private readonly string _valueLeftName;
        private readonly string _valueTopName;

        private bool _canExecute = true;
        public event EventHandler? CanExecuteChanged;

        public ReadMainWindowPositionCommand(string id, double actualWidth, double actualHeight)
        {
            _id = id;
            _actualWidth = actualWidth;
            _actualHeight = actualHeight;

            _keyName = $@"Software\PopupCash\\{id}";
            _valueLeftName = "Left";
            _valueTopName = "Top";
        }

        public bool CanExecute(object? parameter)
        {
            return _canExecute;
        }

        public WindowPosition? Execute(object? parameter)
        {
            if (ReadRegistryValue(_keyName, _valueLeftName) is double leftValue &&
                ReadRegistryValue(_keyName, _valueTopName) is double topValue)
            {
                return new WindowPosition(_id, leftValue, topValue);
            }
            else
            {
                var pos = SystemParameterHelper.GetScreenCenterPoint(_actualWidth, _actualHeight);
                return new WindowPosition(_id, pos.X, pos.Y);
            }
        }

        public void RaiseCanExecuteChanged()
        {
            // CanExecuteChanged 이벤트가 null이 아니면 호출
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        // 레지스트리에서 값을 읽는 메서드
        private double? ReadRegistryValue(string keyName, string valueName)
        {
            try
            {
                using (var key = Registry.CurrentUser.OpenSubKey(keyName))
                {
                    if (key != null)
                    {
                        if (key.GetValue(valueName) is string value)
                        {
                            if (double.TryParse(value, out double position))
                                return position;
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            return null;
        }
    }
}
