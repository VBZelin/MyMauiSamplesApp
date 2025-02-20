namespace MyMauiSamplesApp;

public partial class CancellableTaskDemoPage : BasePage
{
    private readonly CancellableTask _cancellableTask = new();

    public CancellableTaskDemoPage()
    {
        InitializeComponent();
        _cancellableTask.TaskCancelled += OnTaskCancelled;
    }

    private async void OnStartTaskClicked(object sender, EventArgs e)
    {
        statusLabel.Text = "Task running...";
        cancelButton.IsEnabled = true;

        try
        {
            await _cancellableTask.ExecuteAsync(async token =>
            {
                for (int i = 0; i < 10; i++)
                {
                    if (_cancellableTask.IsCancelled) return;
                    await Task.Delay(1000, token);
                    statusLabel.Text = $"Task progress: {i + 1}/10";
                }
                statusLabel.Text = "Task completed.";
            });
        }
        catch (OperationCanceledException)
        {
            statusLabel.Text = "Task cancelled.";
        }
        finally
        {
            cancelButton.IsEnabled = false;
        }
    }

    private void OnCancelTaskClicked(object sender, EventArgs e)
    {
        _cancellableTask.Cancel();
    }

    private void OnTaskCancelled()
    {
        statusLabel.Text = "Task cancelled.";
        cancelButton.IsEnabled = false;
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        _cancellableTask.Cancel();
        _cancellableTask.Dispose();
        await Shell.Current.GoToAsync("..");
    }
}
