namespace MyMauiSamplesApp
{
    public class CancellableTask
    {
        private CancellationTokenSource? _cancellationTokenSource;

        public event Action? TaskCancelled;

        public bool IsCancelled => _cancellationTokenSource is null || _cancellationTokenSource.IsCancellationRequested;

        public CancellableTask() { }

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

        public void Dispose()
        {
            DisposeToken();
        }
    }
}
