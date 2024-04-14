using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;

namespace PopupCash.Common.ViewModels
{
    public abstract partial class ViewModelBase : ObservableObject, IViewModelBase, IDisposable
    {
        private readonly SemaphoreSlim _isBusyLock;
        private bool _disposedValue;

        [ObservableProperty]
        private bool _isBusy;

        protected readonly ILogger logger;

        public ViewModelBase(ILogger<ViewModelBase> loggor)
        //public ViewModelBase(ILogger logger)
        {
            this.logger = loggor;
            this.logger.LogDebug($"{this.GetType().Name} 생성");
            _isBusyLock = new SemaphoreSlim(1, 1);
        }

        public async Task IsBusyFor(Func<Task> unitOfWork, [CallerMemberName] string callerName = "")
        {
            await _isBusyLock.WaitAsync();

            try
            {
                logger.LogDebug($"{callerName} 시작");
                IsBusy = true;

                await unitOfWork();
            }
            finally
            {
                IsBusy = false;
                _isBusyLock.Release();

                logger.LogDebug($"{callerName} 종료");
            }
        }
        public async Task<T> IsBusyFor<T>(Func<Task<T>> unitOfWork, [CallerMemberName] string callerName = "")
        {
            await _isBusyLock.WaitAsync();

            try
            {
                logger.LogDebug($"{callerName} 시작");
                IsBusy = true;

                return await unitOfWork();
            }
            finally
            {
                IsBusy = false;
                _isBusyLock.Release();
                logger.LogDebug($"{callerName} 종료");
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _isBusyLock?.Dispose();
                }

                _disposedValue = true;
            }
        }

        public virtual void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
