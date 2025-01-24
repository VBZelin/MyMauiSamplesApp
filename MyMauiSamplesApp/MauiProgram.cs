using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using Telerik.Maui.Controls.Compatibility;
using Microsoft.Maui.LifecycleEvents;

namespace MyMauiSamplesApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseTelerik()
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		// Uncomment the following code to manage the full Android lifecycle and achieve a transparent status bar
		// #if ANDROID
		//		builder.ConfigureLifecycleEvents(events =>
		//		{
		//			events.AddAndroid(android => android.OnCreate((activity, bundle) =>
		//			{
		//				var window = activity.Window;
		//				if (window is not null)
		//				{
		//					window.SetFlags(Android.Views.WindowManagerFlags.LayoutNoLimits, Android.Views.WindowManagerFlags.LayoutNoLimits);
		//					window.ClearFlags(Android.Views.WindowManagerFlags.TranslucentStatus);
		//				}
		//			}));
		//		});
		// #endif

		RegisterRoutes();

		return builder.Build();
	}

	public static void RegisterRoutes()
	{
		Routing.RegisterRoute(nameof(PreferenceSamplePage), typeof(PreferenceSamplePage));
		Routing.RegisterRoute(nameof(RadCollectionViewPage), typeof(RadCollectionViewPage));
		Routing.RegisterRoute(nameof(TransparentStatusBarPage), typeof(TransparentStatusBarPage));
	}
}
