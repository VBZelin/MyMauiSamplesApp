using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using Telerik.Maui.Controls.Compatibility;

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

		RegisterRoutes();

		return builder.Build();
	}

	public static void RegisterRoutes()
	{
		Routing.RegisterRoute(nameof(PreferenceSamplePage), typeof(PreferenceSamplePage));
		Routing.RegisterRoute(nameof(RadCollectionViewPage), typeof(RadCollectionViewPage));
	}
}
