using System.Diagnostics;
using System.IO;
using System.Windows;

namespace AutoUpdater
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string? _popupCashPath;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Run(() =>
            {
                // 실행된 프로세스의 종료를 대기
                if (IsWaitForTargetExecutable())
                {
                    // 파일 복사 및 실행 작업
                    CopyFilesAndRunTargetExecutable();

                    // PopupCash 실행
                    if (!string.IsNullOrEmpty(_popupCashPath))
                        Process.Start(_popupCashPath);
                }
            });

            // 실행된 프로세스가 종료되면 자신의 프로그램을 종료
            Application.Current.Dispatcher.Invoke(() =>
            {
                Application.Current.Shutdown();
            });
        }

        private bool IsWaitForTargetExecutable()
        {
            // 실행된 프로세스의 종료를 대기하는 로직 구현
            Process[] processes = Process.GetProcessesByName("PopupCash");
            foreach (Process process in processes)
            {
                if (process.MainModule is not null)
                    _popupCashPath = process.MainModule.FileName;

                if (!process.WaitForExit(30000))
                {
                    return false;
                }
            }
            return true;
        }

        private bool StartProcess(string processPath)
        {
            // 프로세스 시작
            Process process = Process.Start(processPath);

            // 3초간 프로세스가 실행되었는지 확인
            bool isProcessRunning = false;
            for (int i = 0; i < 30; i++)
            {
                if (!process.HasExited)
                {
                    isProcessRunning = true;
                    break;
                }

                Thread.Sleep(100); // 100ms 대기
            }

            return isProcessRunning;
        }

        private void CopyFilesAndRunTargetExecutable()
        {
            if (((App)App.Current).Args is not string[] args) throw new Exception("명령줄 인수가 입력되지 않았습니다.");
            if (args[0] is not string version) throw new Exception("버전 정보가 입력되지 않았습니다.");

            // 파일 복사 및 실행 로직 구현
            var localDirPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"PopupCash");
            var updateDirPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @$"PopupCash\{version}");
            var backupPath = System.IO.Path.Combine(localDirPath, "AppDataBackup");

            // 업데이트할 파일 목록을 만든다.
            var updatePaths = Directory.GetFiles(updateDirPath);
            if (updatePaths.Length == 0) throw new Exception("업데이트 할 파일들이 없습니다.");
            List<string> updateFiles = new List<string>();
            foreach (var path in updatePaths)
            {
                // 파일 경로에서 파일 이름과 확장자만 가져오기
                string fileName = Path.GetFileNameWithoutExtension(path);
                string extension = Path.GetExtension(path);

                updateFiles.Add($"{fileName}.{extension}");
            }

            // 업데이트할 파일 목록이 기존 파일에 있는 경우 기존 파일을 백업 폴더로 이동한다.
            MoveFilesWithMatchingNames(updatePaths.ToArray(), localDirPath, backupPath);

            if (!MoveAllFiles(updateDirPath, localDirPath))
            {// 복사가 안 됐을 경우 원본으로 복구 후 중단
                MoveAllFiles(backupPath, localDirPath);
                throw new Exception("신규 버전 복사 실패. \r\n 업데이트에 실패하였습니다.");
            }

            MessageBox.Show($"업데이트가 완료되었습니다.");
            Directory.Delete(updateDirPath, true);
            Directory.Delete(backupPath, true);

        }

        public int MoveFilesWithMatchingNames(string[] updateFileNames, string sourceDir, string targetDir)
        {
            Directory.CreateDirectory(targetDir);
            int fileCount = 0;
            foreach (string fileName in updateFileNames)
            {
                string sourceFilePath = Path.Combine(sourceDir, fileName);
                string targetFilePath = Path.Combine(targetDir, fileName);

                if (File.Exists(sourceFilePath))
                {
                    File.Move(sourceFilePath, targetFilePath, true);
                    fileCount++;
                }
                else
                {
                    // Handle the case where the file does not exist in the source directory
                }
            }
            return fileCount;
        }


        bool MoveAllFiles(string sourcePath, string destinationPath)
        {
            // sourcePath의 모든 파일을 destinationPath로 복사
            Directory.CreateDirectory(destinationPath);
            var sourceFiles = Directory.GetFiles(sourcePath);
            int sourceCopyCount = 0;

            //MessageBox.Show($"{sourceFiles.Length}");
            foreach (string filePath in sourceFiles)
            {
                string fileName = Path.GetFileName(filePath);

                string destFilePath = Path.Combine(destinationPath, fileName);
                File.Move(filePath, destFilePath, true);
                sourceCopyCount++;
            }

            var movedSourceFiles = Directory.GetFiles(sourcePath);
            if (0 != movedSourceFiles.Length)
            {
                return false;
            }

            return true;

        }
    }
}