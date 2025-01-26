using UIKit;

namespace MyMauiSamplesApp;

public static partial class SafeAreaHelper
{
    public static partial Thickness GetSafeAreaInsets()
    {
        if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
        {
            var window = UIApplication.SharedApplication
                .ConnectedScenes
                .OfType<UIWindowScene>()
                .SelectMany(scene => scene.Windows)
                .FirstOrDefault(w => w.IsKeyWindow);

            if (window != null)
            {
                var insets = window.SafeAreaInsets;
                return new Thickness(insets.Left, insets.Top, insets.Right, insets.Bottom);
            }
        }

        return new Thickness(0);
    }
}
