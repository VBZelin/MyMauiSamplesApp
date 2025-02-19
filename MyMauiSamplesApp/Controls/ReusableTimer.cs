namespace MyMauiSamplesApp
{
    public class ReusableTimer : IDisposable
    {
        private Timer? _timer;
        private TimeSpan _interval;
        private readonly Func<Task> _callback;
        private bool _isRunning;
        private readonly object _lock = new();

        public ReusableTimer(TimeSpan interval, Func<Task> callback)
        {
            _interval = interval;
            _callback = callback ?? throw new ArgumentNullException(nameof(callback));
        }

        public void Start()
        {
            lock (_lock)
            {
                if (_isRunning)
                    return;

                _timer = new Timer(async _ => await ExecuteCallbackAsync(), null, _interval, _interval);
                _isRunning = true;
            }
        }

        public void Stop()
        {
            lock (_lock)
            {
                _timer?.Change(Timeout.Infinite, Timeout.Infinite);
                _isRunning = false;
            }
        }

        public void Reset(TimeSpan newInterval)
        {
            lock (_lock)
            {
                _interval = newInterval;
                Stop();
                Start();
            }
        }

        private async Task ExecuteCallbackAsync()
        {
            try
            {
                await _callback.Invoke();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Timer callback exception: {ex.Message}");
            }
        }

        public void Dispose()
        {
            lock (_lock)
            {
                _timer?.Dispose();
                _isRunning = false;
            }
        }
    }
}
