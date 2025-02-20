namespace MyMauiSamplesApp;

public partial class ReusableTimerSamplePage : BasePage
{
    private readonly ReusableTimer _reusableTimer;
    private bool _isTimerRunning = false;

    public ReusableTimerSamplePage()
    {
        InitializeComponent();
        _reusableTimer = new ReusableTimer(TimeSpan.FromSeconds(1), UpdateTime);
    }

    private async Task UpdateTime()
    {
        await Dispatcher.DispatchAsync(() =>
        {
            timerLabel.Text = $"Updated at: {DateTime.Now:T}";
        });
    }

    private void OnToggleTimerButtonClicked(object sender, EventArgs e)
    {
        if (_isTimerRunning)
        {
            _reusableTimer.Stop();
            toggleTimerButton.Text = "Start Timer";
            timerLabel.Text = "Timer stopped";
        }
        else
        {
            _reusableTimer.Start();
            toggleTimerButton.Text = "Stop Timer";
            timerLabel.Text = "Timer started";
        }

        _isTimerRunning = !_isTimerRunning;
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        _reusableTimer.Dispose();
        await Shell.Current.GoToAsync("..");
    }
}
