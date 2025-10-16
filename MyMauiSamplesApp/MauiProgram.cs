using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using Serilog;
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
        // -----------------------------------------------------------------------------
        // Configure .NET MAUI built-in logging providers
        // -----------------------------------------------------------------------------

        var logPath = Path.Combine(FileSystem.AppDataDirectory, "app.log");

        builder.Logging
            .AddDebug()
            .AddConsole()
            .AddFile(logPath)
            //.AddFilter(typeof(ConfigAwaitSamplePage).FullName, LogLevel.Information)
            .SetMinimumLevel(LogLevel.Trace);

        // -----------------------------------------------------------------------------
        // Configure Serilog for logging to multiple targets
        // -----------------------------------------------------------------------------

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Debug()
            .WriteTo.Console()
            .WriteTo.File(
                path: logPath,
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 7,
                shared: true)
            //.Filter.ByIncludingOnly(e =>
            //e.Properties.ContainsKey("SourceContext") &&
            //e.Properties["SourceContext"].ToString().Contains(typeof(ConfigAwaitSamplePage).FullName!) &&
            //e.Level >= LogEventLevel.Information)
            .CreateLogger();

        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog();
#endif

#if ANDROID
        builder.ConfigureLifecycleEvents(events =>
        {
            events.AddAndroid(android => android.OnCreate((activity, bundle) =>
            {
                var window = activity.Window;
                if (window is not null)
                {
                    window.SetFlags(
                        Android.Views.WindowManagerFlags.LayoutNoLimits,
                        Android.Views.WindowManagerFlags.LayoutNoLimits);
                    window.ClearFlags(Android.Views.WindowManagerFlags.TranslucentStatus);
                }
            }));
        });
#endif

        RegisterRoutes();

        return builder.Build();
    }

    public static void RegisterRoutes()
    {
        Routing.RegisterRoute(nameof(PreferenceSamplePage), typeof(PreferenceSamplePage));
        Routing.RegisterRoute(nameof(RadCollectionViewPage), typeof(RadCollectionViewPage));
        Routing.RegisterRoute(nameof(TransparentStatusBarPage), typeof(TransparentStatusBarPage));
        Routing.RegisterRoute(nameof(SideDrawerPage), typeof(SideDrawerPage));
        Routing.RegisterRoute(nameof(CancellableTaskSamplePage), typeof(CancellableTaskSamplePage));
        Routing.RegisterRoute(nameof(ReusableTimerSamplePage), typeof(ReusableTimerSamplePage));
        Routing.RegisterRoute(nameof(TabViewSamplePage), typeof(TabViewSamplePage));
        Routing.RegisterRoute(nameof(PopupPage), typeof(PopupPage));
        Routing.RegisterRoute(nameof(ConfigAwaitSamplePage), typeof(ConfigAwaitSamplePage));
    }
}
