namespace MyMauiSamplesApp;

/// <summary>
/// Mock class to access application services.
/// </summary>
public static class AppServices
{
    /// <summary>
    /// Gets or sets the service provider for accessing application services.
    /// </summary>
    public static IServiceProvider? Services { get; set; } = IPlatformApplication.Current?.Services;

    /// <summary>
    /// Gets a service of the specified type from the service provider.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T? GetService<T>() => (Services is IServiceProvider sp) ? sp.GetService<T>() : default;
}
