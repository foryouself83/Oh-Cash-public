using System.Reflection;
using Microsoft.Win32;

namespace PopupCash.Models.Commands.Impl
{
    public class RegisterStartTryIconCommand : TrayIconCommandBase<RegisterStartTryIconCommand>
    {
        private static readonly string _startupRegPath =
            @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";

        private static readonly string _progamName = "PopupCash";

        public override void Execute(object? parameter)
        {
            if (Assembly.GetEntryAssembly() is not Assembly assembly) return;
            var exePath = assembly.Location;

            AddStartupProgram(_progamName, exePath);
        }


        public override bool CanExecute(object? parameter)
        {
            using var regKey = GetRegKey(_startupRegPath, true);

            if (regKey == null || regKey.GetValue(_progamName) is null) return false;

            regKey.Close();

            return true;
        }

        private RegistryKey? GetRegKey(string regPath, bool writable)
        {
            return Registry.CurrentUser.OpenSubKey(regPath, writable);
        }

        public void AddStartupProgram(string programName, string executablePath)
        {
            using var regKey = GetRegKey(_startupRegPath, true);
            try
            {
                // 키가 이미 등록돼 있지 않을때만 등록
                if (regKey?.GetValue(programName) == null)
                    regKey?.SetValue(programName, executablePath);

                regKey?.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
