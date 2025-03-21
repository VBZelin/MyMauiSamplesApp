﻿using Timer = System.Timers.Timer;

namespace MyMauiSamplesApp
{
    public partial class ReusableTimer
    {
        private readonly Timer _timer;
        private readonly Func<Task> _callback;
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
        }

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

        public void Dispose()
        {
            Stop();
            _timer.Elapsed -= EventHandler;
            _timer.Close();
        }
    }
}
