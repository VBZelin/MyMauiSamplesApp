namespace MyMauiSamplesApp;

public partial class TransparentStatusBarPage : ContentPage
{
    public TransparentStatusBarPage()
    {
        InitializeComponent();
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

#if ANDROID
        var window = Platform.CurrentActivity?.Window;
        if (window != null)
        {
            window.SetFlags(Android.Views.WindowManagerFlags.LayoutNoLimits, Android.Views.WindowManagerFlags.LayoutNoLimits);
            window.ClearFlags(Android.Views.WindowManagerFlags.TranslucentStatus);
        }
#endif
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

#if ANDROID
        var window = Platform.CurrentActivity?.Window;
        if (window != null)
        {
            window.ClearFlags(Android.Views.WindowManagerFlags.LayoutNoLimits);
            window.AddFlags(Android.Views.WindowManagerFlags.TranslucentStatus);
        }
#endif
    }
}