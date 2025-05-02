using Timer = System.Timers.Timer;

namespace MyMauiSamplesApp
{
    public class ReusableTimer
    {
        private readonly Timer _timer;
        private readonly Func<Task> _callback;
        private readonly SynchronizationContext? _syncContext;
        private bool _callbackRunning = false;

        private bool AutoReset => _timer.AutoReset;

        public ReusableTimer(TimeSpan interval, Func<Task> callback, bool autoReset = true)
        {
            _callback = callback ?? throw new ArgumentNullException(nameof(callback));
            _timer = new Timer(interval.TotalMilliseconds)
            {
                AutoReset = autoReset
            };
            _timer.Elapsed += EventHandler;
            _syncContext = SynchronizationContext.Current;
        }

        public ReusableTimer(TimeSpan interval, Action callback, bool autoReset = true)
            : this(interval, () =>
            {
                callback();
                return Task.CompletedTask;
            }, autoReset)
        { }

        public void Start() => _timer.Start();

        public void Stop() => _timer.Stop();

        public void Reset(TimeSpan newInterval)
        {
            Stop();
            _timer.Interval = newInterval.TotalMilliseconds;
            Start();
        }

        private async void EventHandler(object? sender, EventArgs e)
        {
            await ExecuteCallbackAsync();
        }

        private async Task ExecuteCallbackAsync()
        {
            if (_callbackRunning) return;
            _callbackRunning = true;

            _timer.Stop();

            try
            {
                if (_syncContext is not null)
                {
                    var tcs = new TaskCompletionSource();
                    _syncContext.Post(async _ =>
                    {
                        try
                        {
                            await _callback.Invoke();
                            tcs.SetResult();
                        }
                        catch (Exception ex)
                        {
                            tcs.SetException(ex);
                        }
                    }, null);
                    await tcs.Task;
                }
                else
                {
                    await _callback.Invoke();
                }
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

        public void Dispose()
        {
            Stop();
            _timer.Elapsed -= EventHandler;
            _timer.Close();
        }
    }
}
