using System.Diagnostics;
using System.ComponentModel;

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

    private readonly Stopwatch _stopwatch = new();

    public CancellableTaskSamplePage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    private async void OnStartTaskClicked(object sender, EventArgs e)
    {
        IsBusy = true;
        statusLabel.Text = "Task status: Running...";
        _stopwatch.Restart();

        try
        {
            await cancellableTask.ExecuteAsync(async (token) =>
            {
                double piApproximation = 1.0;
                int i = 1;

                while (!cancellableTask.IsCancelled)
                {
                    double sign = (i % 2 == 0) ? 1 : -1;
                    piApproximation += sign / (2 * i + 1);
                    i++;
                    double piValue = piApproximation * 4;

                    if (_stopwatch.ElapsedMilliseconds >= 1000)
                    {
                        statusLabel.Text = $"Pi: {piValue:F30}";
                        _stopwatch.Restart();
                    }

                    await Task.Delay(10, token);
                }
            });
        }
        catch (OperationCanceledException)
        {
            statusLabel.Text = "Task status: Cancelled";
        }
        finally
        {
            IsBusy = false;
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
