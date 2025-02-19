namespace MyMauiSamplesApp
{
    public class CancellableTask : IDisposable
    {
        private CancellationTokenSource? _cancellationTokenSource;

        public event Action? TaskCancelled;

        public bool IsCancelled => _cancellationTokenSource is null || _cancellationTokenSource.IsCancellationRequested;

        private bool _disposed;

        public CancellableTask()
        {
            Reset();
        }

        ~CancellableTask() => Dispose(disposing: false);

        public void Reset()
        {
            Cancel();
            DisposeToken();
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void Cancel()
        {
            if (_cancellationTokenSource is not null && !_cancellationTokenSource.IsCancellationRequested)
            {
                _cancellationTokenSource.Cancel();
                TaskCancelled?.Invoke();
            }
        }

        public async Task ExecuteAsync(Func<CancellationToken, Task> taskFunc)
        {
            Reset();

            try
            {
                if (_cancellationTokenSource is not null)
                {
                    await taskFunc(_cancellationTokenSource.Token);
                }
                DisposeToken();
            }
            catch (OperationCanceledException)
            {
                throw;
            }
        }

        private void DisposeToken()
        {
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                DisposeToken();
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
