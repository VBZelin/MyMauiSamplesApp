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
            // ❌ Bad
            // await cancellableTask.ExecuteAsync(RunPiApproximationOnUiThread);

            // ✅ Alternative
            await cancellableTask.ExecuteAsync(RunPiApproximationOnUiThreadWithYield);

            // ✅ Good
            // await cancellableTask.ExecuteAsync(RunPiApproximationInBackground);

            if (cancellableTask.IsCancelled)
            {
                statusLabel.Text = "Task status: Cancelled";
            }
            else
            {
                statusLabel.Text = "Task status: Completed";
            }
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

    /// <summary>
    /// Approximates π using the Leibniz series until cancelled.
    /// Blocks the UI by default, but if <paramref name="useAsyncYield"/> is true,
    /// yields each iteration to keep the UI responsive.
    /// </summary>
    private async Task RunPiApproximationLoop(bool useAsyncYield = false)
    {
        var stopwatch = Stopwatch.StartNew();
        double piApproximation = 1.0;
        int i = 1;

        while (!cancellableTask.IsCancelled)
        {
            if (useAsyncYield)
            {
                // Real pause so UI can process input/layout/paint
                await Task.Delay(1);
            }

            double sign = (i % 2 == 0) ? 1.0 : -1.0;
            piApproximation += sign / (2 * i + 1);
            i++;
            double piValue = piApproximation * 4;

            if (stopwatch.ElapsedMilliseconds >= 1000)
            {
                statusLabel.Dispatcher.Dispatch(() =>
                {
                    statusLabel.Text = $"Pi: {piValue:F30}";
                });
                stopwatch.Restart();
            }
        }
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
