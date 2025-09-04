using System.ComponentModel;
using System.Diagnostics;

namespace MyMauiSamplesApp;

public partial class CancellableTaskSamplePage : BasePage, INotifyPropertyChanged
{
    private bool _isBusy;

    public new bool IsBusy
    {
        get => _isBusy;
        protected set
        {
            if (_isBusy != value)
            {
                _isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
            }
        }
    }

    public CancellableTaskSamplePage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    private async void OnStartTaskClicked(object sender, EventArgs e)
    {
        IsBusy = true;
        statusLabel.Text = "Task status: Running...";

        try
        {
            // ✅ Run the Pi approximation on a dedicated background thread via ExecuteAsync.
            await cancellableTask.ExecuteAsync(RunPiOnDedicatedThreadAsync);
        }
        finally
        {
            IsBusy = false;
        }
    }

    // ✅ Good: runs on background thread pool, UI stays responsive
    private Task RunPiApproximationInBackground(CancellationToken token)
    {
        return Task.Run(() => RunPiApproximationLoop(), token);
    }

    // ❌ Bad: runs directly on UI thread, blocks until cancelled
    private Task RunPiApproximationOnUiThread(CancellationToken _)
    {
        return RunPiApproximationLoop();
    }

    // ✅ Alternative: runs on UI thread but yields cooperatively to keep UI responsive
    private Task RunPiApproximationOnUiThreadWithYield(CancellationToken _)
    {
        return RunPiApproximationLoop(useAsyncYield: true);
    }

    // ✅ Alternative: runs on a dedicated thread, keeping UI responsive, but not common
    private async Task RunPiOnDedicatedThreadAsync(CancellationToken _)
    {
        var tcs = new TaskCompletionSource<object?>(
            TaskCreationOptions.RunContinuationsAsynchronously);

        var thread = new Thread(() =>
        {
            try
            {
                RunPiApproximationLoop().GetAwaiter().GetResult();
                tcs.TrySetResult(null);
            }
            catch (Exception ex)
            {
                tcs.TrySetException(ex);
            }
        })
        { IsBackground = true, Name = "PiWorkerThread" };

        thread.Start();
        await tcs.Task.ConfigureAwait(false);
    }

    /// <summary>
    /// Approximates π (Leibniz) until <c>cancellableTask.IsCancelled</c> is true.
    /// By default runs synchronously on the current thread (blocks UI).
    /// If <paramref name="useAsyncYield"/> is true, awaits a small delay each iteration
    /// to keep the UI responsive while computing.
    /// </summary>
    private async Task RunPiApproximationLoop(bool useAsyncYield = false, int delayMs = 1)
    {
        var stopwatch = Stopwatch.StartNew();
        double piApproximation = 1.0;
        int i = 1;

        while (!cancellableTask.IsCancelled)
        {
            if (useAsyncYield)
            {
                await Task.Delay(delayMs);
            }

            double sign = (i % 2 == 0) ? 1.0 : -1.0;
            piApproximation += sign / (2 * i + 1);
            i++;

            if (stopwatch.ElapsedMilliseconds >= 1000)
            {
                double piValue = piApproximation * 4;
                statusLabel.Dispatcher.Dispatch(() =>
                {
                    statusLabel.Text = $"Pi: {piValue:F30}";
                });
                stopwatch.Restart();
            }
        }

        // Reached when cancelled: show final status
        statusLabel.Dispatcher.Dispatch(() =>
        {
            statusLabel.Text = "Task status: Cancelled";
        });
    }

    private void OnCancelTaskClicked(object sender, EventArgs e)
    {
        cancellableTask.Cancel();
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}
