
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
            page.UpdateIgnoreSafeArea(isFullScreen);
        }
    }

    public static readonly BindableProperty BodyTemplateProperty =
    BindableProperty.Create(
        nameof(BodyTemplate),
        typeof(ControlTemplate),
        typeof(BasePage),
        defaultValue: null);

    public ControlTemplate BodyTemplate
    {
        get => (ControlTemplate)GetValue(BodyTemplateProperty);
        set => SetValue(BodyTemplateProperty, value);
    }

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
            UpdateIgnoreSafeArea(true);
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        if (UseFullScreen)
        {
            UpdateAndroidWindowFlags(false);
            UpdateIgnoreSafeArea(false);
        }
    }

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

    public void UpdateIgnoreSafeArea(bool ignoreSafeArea)
    {
        if (mainGrid is not null)
        {
            mainGrid.IgnoreSafeArea = ignoreSafeArea;
        }
    }
}
