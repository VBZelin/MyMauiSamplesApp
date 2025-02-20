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
        StatusLabel.Text = "Task running...";
        CancelButton.IsEnabled = true;

        try
        {
            await _cancellableTask.ExecuteAsync(async token =>
            {
                for (int i = 0; i < 10; i++)
                {
                    if (token.IsCancellationRequested) return;
                    await Task.Delay(1000, token);
                    StatusLabel.Text = $"Task progress: {i + 1}/10";
                }
                StatusLabel.Text = "Task completed.";
            });
        }
        catch (OperationCanceledException)
        {
            StatusLabel.Text = "Task cancelled.";
        }
        finally
        {
            CancelButton.IsEnabled = false;
        }
    }

    private void OnCancelTaskClicked(object sender, EventArgs e)
    {
        _cancellableTask.Cancel();
    }

    private void OnTaskCancelled()
    {
        StatusLabel.Text = "Task cancelled.";
        CancelButton.IsEnabled = false;
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        _cancellableTask.Cancel();
        _cancellableTask.Dispose();
        await Shell.Current.GoToAsync("..");
    }
}
