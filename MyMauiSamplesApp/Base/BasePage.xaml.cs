
namespace MyMauiSamplesApp;

public partial class BasePage : ContentPage
{
    public static readonly BindableProperty UseFullScreenProperty =
    BindableProperty.Create(
        nameof(UseFullScreen),
        typeof(bool),
        typeof(BasePage),
        defaultValue: false,
        propertyChanged: OnUseFullScreenChanged);

    public bool UseFullScreen
    {
        get => (bool)GetValue(UseFullScreenProperty);
        set => SetValue(UseFullScreenProperty, value);
    }

    private static void OnUseFullScreenChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is BasePage page && newValue is bool isFullScreen)
        {
            page.UpdateAndroidWindowFlags(isFullScreen);
        }
    }

    public static readonly BindableProperty StatusBarColorProperty =
            BindableProperty.Create(
                nameof(StatusBarColor),
                typeof(Color),
                typeof(BasePage),
                Colors.Transparent);

    public Color StatusBarColor
    {
        get => (Color)GetValue(StatusBarColorProperty);
        set => SetValue(StatusBarColorProperty, value);
    }

    public bool IsInNavigationStack => Shell.Current.Navigation.NavigationStack.Contains(this)
        || Shell.Current.Navigation.ModalStack.Contains(this);

    protected readonly CancellableTask cancellableTask = new();

    public BasePage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (UseFullScreen)
        {
            UpdateAndroidWindowFlags(true);
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        if (UseFullScreen)
        {
            UpdateAndroidWindowFlags(false);
        }
    }

    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);

        if (!IsInNavigationStack)
        {
            ReleaseResources();
            cancellableTask?.Cancel();
        }
    }

    protected virtual void ReleaseResources() { }

    public void UpdateAndroidWindowFlags(bool enabled)
    {
#if ANDROID
        var window = Platform.CurrentActivity?.Window;
        if (window == null) return;

        if (enabled)
        {
            window.SetFlags(Android.Views.WindowManagerFlags.LayoutNoLimits, Android.Views.WindowManagerFlags.LayoutNoLimits);
            window.ClearFlags(Android.Views.WindowManagerFlags.TranslucentStatus);
        }
        else
        {
            window.ClearFlags(Android.Views.WindowManagerFlags.LayoutNoLimits);
            window.AddFlags(Android.Views.WindowManagerFlags.TranslucentStatus);
        }
#endif
    }
}
