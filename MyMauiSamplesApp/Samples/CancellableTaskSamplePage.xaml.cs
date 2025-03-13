namespace MyMauiSamplesApp;

public partial class CancellableTaskSamplePage : BasePage
{
    private readonly CancellableTask _cancellableTask = new();

    public CancellableTaskSamplePage()
    {
        InitializeComponent();
    }

    private async void OnStartTaskClicked(object sender, EventArgs e)
    {
        startButton.IsEnabled = false;
        cancelButton.IsEnabled = true;
        statusLabel.Text = "Task status: Running...";

        try
        {
            await _cancellableTask.ExecuteAsync(async (token) =>
            {
                double piApproximation = 1.0;
                int i = 1;

                while (!token.IsCancellationRequested)
                {
                    double sign = (i % 2 == 0) ? 1 : -1;
                    piApproximation += sign / (2 * i + 1);
                    i++;
                    double piValue = piApproximation * 4;

                    statusLabel.Text = $"Pi: {piValue:F30}";

                    await Task.Delay(200, token);
                }
            });
        }
        catch (OperationCanceledException)
        {
            statusLabel.Text = "Task status: Cancelled";
        }
        finally
        {
            startButton.IsEnabled = true;
            cancelButton.IsEnabled = false;
        }
    }

    private void OnCancelTaskClicked(object sender, EventArgs e)
    {
        _cancellableTask.Cancel();
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        _cancellableTask.Cancel();
        _cancellableTask.Dispose();
        await Shell.Current.GoToAsync("..");
    }
}
