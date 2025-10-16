using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using Telerik.Maui.Controls;

namespace MyMauiSamplesApp;

public class CodeSample
{
    public string Title { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}

public class Message
{
    public string Text { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.Now;
    public Color TextColor { get; set; } = Colors.Black; // default black
}

public partial class ConfigAwaitSamplePage : BasePage
{
    public ObservableCollection<CodeSample> CodeSamples { get; set; } = [];
    public ObservableCollection<Message> Messages { get; set; } = [];

    // Palette (console-style)
    private readonly Color _infoColor = Color.FromArgb("#16A34A"); // info = green

    private readonly Color _warningColor = Color.FromArgb("#F59E0B"); // warning = orange
    private readonly Color _errorColor = Color.FromArgb("#DC2626"); // error = red

    public ConfigAwaitSamplePage()
    {
        InitializeComponent();
        BindingContext = this;

        SeedCodeSamples();
    }

    private void SeedCodeSamples()
    {
        CodeSamples.Add(new CodeSample
        {
            Title = "Default Await",
            Code =
                    """
                    private async Task RunDefaultAwaitAsync()
                    {
                        AppendMessage($"Before await, thread: {Environment.CurrentManagedThreadId}");
                        await Task.Delay(1000); // no ConfigureAwait
                        AppendMessage($"After await, thread: {Environment.CurrentManagedThreadId}");
                    }
                    """
        });

        CodeSamples.Add(new CodeSample
        {
            Title = "ConfigureAwait(false)",
            Code =
                    """
                    private async Task RunConfigureAwaitFalseAsync()
                    {
                        AppendMessage($"Before await, thread: {Environment.CurrentManagedThreadId}");
                        await Task.Delay(1000).ConfigureAwait(false);
                        AppendMessage($"After await, thread: {Environment.CurrentManagedThreadId}");
                    }
                    """
        });

        CodeSamples.Add(new CodeSample
        {
            Title = "Run Method That Contains ConfigureAwait",
            Code =
                    """
                    private async Task RunMethodContainsConfigureAwaitAsync()
                    {
                        AppendMessage($"Before calling HelperMethodContainsConfigureAwait, thread: {Environment.CurrentManagedThreadId}");
                        await HelperMethodContainsConfigureAwaitAsync();
                        AppendMessage($"After calling HelperMethodContainsConfigureAwait, thread: {Environment.CurrentManagedThreadId}");
                    }

                    private async Task HelperMethodContainsConfigureAwaitAsync()
                    {
                        AppendMessage($"Inside helper (before await), thread: {Environment.CurrentManagedThreadId}");
                        await Task.Delay(1000).ConfigureAwait(false);
                        AppendMessage($"Inside helper (after await), thread: {Environment.CurrentManagedThreadId}");
                    }
                    """
        });
    }

    private void AppendMessage(string text, Color? color = null)
    {
        Dispatcher.Dispatch(() =>
        {
            var timestamp = DateTime.Now;
            var message = new Message
            {
                Text = text,
                Timestamp = timestamp,
                TextColor = color ?? Colors.Black // default if none provided
            };

            Messages.Add(message);

            // Structured logging with timestamp
            SampleLogger.Logger?.LogTrace(
                "[{Timestamp:yyyy-MM-dd HH:mm:ss}] {Message}",
                timestamp, text);

            SampleLogger.Logger?.LogInformation(
                "[{Timestamp:yyyy-MM-dd HH:mm:ss}] {Message}",
                timestamp, text);
        });
    }

    private async void OnRunDemoClicked(object sender, EventArgs e)
    {
        if (tabView.SelectedItem is not TabViewItem tabItem ||
            tabItem.BindingContext is not CodeSample selected)
        {
            Messages.Clear();
            AppendMessage("⚠️ No sample selected.", _warningColor);
            return;
        }

        Messages.Clear();
        AppendMessage($"ℹ️ Starting demo… ({selected.Title})", _infoColor);

        try
        {
            switch (selected.Title)
            {
                case "Default Await":
                    await RunDefaultAwaitAsync();
                    break;

                case "ConfigureAwait(false)":
                    await RunConfigureAwaitFalseAsync();
                    break;

                case "Run Method That Contains ConfigureAwait":
                    await RunMethodContainsConfigureAwaitAsync();
                    break;

                default:
                    AppendMessage("⚠️ Unrecognized sample — running all demos.", _warningColor);
                    await RunDefaultAwaitAsync();
                    await RunConfigureAwaitFalseAsync();
                    await RunMethodContainsConfigureAwaitAsync();
                    break;
            }

            AppendMessage("✅ Demo complete.", _infoColor);
        }
        catch (Exception ex)
        {
            AppendMessage($"❌ Error: {ex.Message}", _errorColor);
        }
    }

    private async Task RunDefaultAwaitAsync()
    {
        var beforeThread = Environment.CurrentManagedThreadId;
        AppendMessage($"ℹ️ [DefaultAwait] Before await, thread: {beforeThread}", _infoColor);

        await Task.Delay(500);

        var afterThread = Environment.CurrentManagedThreadId;
        var sameThread = beforeThread == afterThread;
        var color = sameThread ? _infoColor : _warningColor;
        var icon = sameThread ? "✔️" : "🔄";
        AppendMessage($"{icon} [DefaultAwait] After await, thread: {afterThread}", color);
    }

    private async Task RunConfigureAwaitFalseAsync()
    {
        var beforeThread = Environment.CurrentManagedThreadId;
        AppendMessage($"ℹ️ [ConfigureAwait(false)] Before await, thread: {beforeThread}", _infoColor);

        await Task.Delay(500).ConfigureAwait(false);

        var afterThread = Environment.CurrentManagedThreadId;
        var sameThread = beforeThread == afterThread;
        var color = sameThread ? _infoColor : _warningColor;
        var icon = sameThread ? "✔️" : "🔄";
        AppendMessage($"{icon} [ConfigureAwait(false)] After await, thread: {afterThread}", color);
    }

    private async Task RunMethodContainsConfigureAwaitAsync()
    {
        var beforeThread = Environment.CurrentManagedThreadId;
        AppendMessage($"ℹ️ Before calling helper, thread: {beforeThread}", _infoColor);

        await HelperMethodContainsConfigureAwaitAsync();

        var afterThread = Environment.CurrentManagedThreadId;
        var sameThread = beforeThread == afterThread;
        var color = sameThread ? _infoColor : _warningColor;
        var icon = sameThread ? "✔️" : "🔄";
        AppendMessage($"{icon} After calling helper, thread: {afterThread}", color);
    }

    private async Task HelperMethodContainsConfigureAwaitAsync()
    {
        var beforeThread = Environment.CurrentManagedThreadId;
        AppendMessage($"ℹ️ Inside helper (before await), thread: {beforeThread}", _infoColor);

        await Task.Delay(1000).ConfigureAwait(false);

        var afterThread = Environment.CurrentManagedThreadId;
        var sameThread = beforeThread == afterThread;
        var color = sameThread ? _infoColor : _warningColor;
        var icon = sameThread ? "✔️" : "🔄";
        AppendMessage($"{icon} Inside helper (after await), thread: {afterThread}", color);
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}
