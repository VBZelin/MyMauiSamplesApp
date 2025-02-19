using Timer = System.Timers.Timer;

namespace MyMauiSamplesApp
{
    public class ReusableTimer : IDisposable
    {
        private readonly Timer _timer;
        private readonly Func<Task> _callback;
        private bool _callbackRunning = false;
        private bool _disposed = false;

        private bool AutoReset => _timer.AutoReset;

        public bool Enabled
        {
            get => _timer.Enabled;
            set
            {
                if (value)
                {
                    _timer.Enabled = true;
                    Start();
                }
                else
                {
                    _timer.Enabled = false;
                    Stop();
                }
            }
        }

        public ReusableTimer(TimeSpan interval, Func<Task> callback, bool autoReset = true)
        {
            _callback = callback ?? throw new ArgumentNullException(nameof(callback));
            _timer = new Timer(interval.TotalMilliseconds)
            {
                AutoReset = autoReset
            };
            _timer.Elapsed += async (sender, e) => await ExecuteCallbackAsync();
        }

        ~ReusableTimer() => Dispose(false);

        public void Start()
        {
            if (!Enabled)
            {
                _timer.Start();
            }
        }

        public void Stop()
        {
            if (Enabled)
            {
                _timer.Stop();
            }
        }

        public void Reset(TimeSpan newInterval)
        {
            Stop();
            _timer.Interval = newInterval.TotalMilliseconds;
            Start();
        }

        private async Task ExecuteCallbackAsync()
        {
            if (_callbackRunning) return;
            _callbackRunning = true;

            _timer.Stop();

            try
            {
                await _callback.Invoke();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Timer callback exception: {ex.Message}");
            }
            finally
            {
                _callbackRunning = false;
                if (AutoReset) _timer.Start();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                Stop();
                _timer.Dispose();
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
