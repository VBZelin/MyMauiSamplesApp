using Telerik.Maui.Controls;

namespace MyMauiSamplesApp;

public partial class BottomSheet : ContentView
{
    public RadSideDrawer Drawer => GetTemplateChild("drawer") as RadSideDrawer ?? throw new NullReferenceException("drawer");

    public static readonly BindableProperty DrawerLengthProperty =
    BindableProperty.Create(
        nameof(DrawerLength),
        typeof(double),
        typeof(BottomSheet),
        0.0);

    public double DrawerLength
    {
        get => (double)GetValue(DrawerLengthProperty);
        set => SetValue(DrawerLengthProperty, value);
    }

    public static readonly BindableProperty DrawerLocationProperty =
    BindableProperty.Create(
        nameof(DrawerLocation),
        typeof(SideDrawerLocation),
        typeof(BottomSheet),
        SideDrawerLocation.Bottom);

    public SideDrawerLocation DrawerLocation
    {
        get => (SideDrawerLocation)GetValue(DrawerLocationProperty);
        set => SetValue(DrawerLocationProperty, value);
    }

    public static readonly BindableProperty DrawerTransitionProperty =
    BindableProperty.Create(
        nameof(DrawerTransition),
        typeof(SideDrawerTransitionType),
        typeof(BottomSheet),
        SideDrawerTransitionType.SlideInOnTop);

    public SideDrawerTransitionType DrawerTransition
    {
        get => (SideDrawerTransitionType)GetValue(DrawerTransitionProperty);
        set => SetValue(DrawerTransitionProperty, value);
    }

    public BottomSheet()
    {
        InitializeComponent();
    }

    public void Open()
    {
        Drawer.IsOpen = true;
    }

    private void OnHandlerChanged(object sender, EventArgs e)
    {
        UpdateDrawer();
    }

    private void UpdateDrawer()
    {
#if IOS || MACCATALYST
        var platformView = Drawer.Handler?.PlatformView;

        if (platformView is TelerikUI.TKSideDrawerView platformSideDrawer)
        {
            var sideDrawer = platformSideDrawer.SideDrawers[0];
            sideDrawer.Style.HeaderHeight = 0;
            sideDrawer.Style.FooterHeight = 0;
        }
#endif
    }
}
