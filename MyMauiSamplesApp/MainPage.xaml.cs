namespace MyMauiSamplesApp;

public partial class MainPage : BasePage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private void OnGCCollectClicked(object sender, EventArgs e)
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect(); // Call twice for better cleanup
    }

    private async void OnPreferenceClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(PreferenceSamplePage));
    }

    private async void OnRadCollectionViewClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(RadCollectionViewPage));
    }

    private async void OnTransparentStatusBarClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(TransparentStatusBarPage));
    }

    private async void OnSideDrawerClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(SideDrawerPage));
    }

    private async void OnCancellableTaskClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(CancellableTaskSamplePage));
    }

    private async void OnReusableTimerClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ReusableTimerSamplePage));
    }

    private async void OnTabViewClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(TabViewSamplePage));
    }

    private async void OnPopupClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(PopupPage));
    }
}
