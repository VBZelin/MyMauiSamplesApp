using Microsoft.Extensions.Logging;

namespace MyMauiSamplesApp;

/// <summary>
/// Provides a logger instance for logging within the application.
/// </summary>
public class SampleLogger
{
    /// <summary>
    /// Gets the <see cref="ILogger"/> instance associated with <see cref="SampleLogger"/>.
    /// </summary>
    public static ILogger? Logger { get; } = AppServices.GetService<ILogger<SampleLogger>>();
}
