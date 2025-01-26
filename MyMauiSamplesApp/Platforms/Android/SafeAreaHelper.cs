namespace MyMauiSamplesApp;

public static partial class SafeAreaHelper
{
    public static partial Thickness GetSafeAreaInsets()
    {
        var safeAreaInsets = new Thickness(0, GetStatusBarHeight(), 0, GetNavigationBarHeight());
        return safeAreaInsets;
    }

    public static double GetStatusBarHeight()
    {
        return GetSystemBarHeight("status_bar_height");
    }

    public static double GetNavigationBarHeight()
    {
        return GetSystemBarHeight("navigation_bar_height");
    }

    private static double GetSystemBarHeight(string resourceName)
    {
        var context = Platform.CurrentActivity ?? throw new NullReferenceException("Activity is null");

        var resources = context.Resources;
        if (resources == null)
            return 0;

        int resourceId = resources.GetIdentifier(resourceName, "dimen", "android");
        if (resourceId > 0)
        {
            var displayMetrics = resources.DisplayMetrics;
            if (displayMetrics == null)
                return 0;

            return resources.GetDimensionPixelSize(resourceId) / displayMetrics.Density;
        }

        return 0;
    }
}
