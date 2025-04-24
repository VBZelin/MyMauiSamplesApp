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
            await cancellableTask.ExecuteAsync(token =>
            {
                return Task.Run(() =>
                {
                    var stopwatch = Stopwatch.StartNew();
                    double piApproximation = 1.0;
                    int i = 1;

                    while (!token.IsCancellationRequested)
                    {
                        double sign = (i % 2 == 0) ? 1 : -1;
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

                    token.ThrowIfCancellationRequested();
                }, token);
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
