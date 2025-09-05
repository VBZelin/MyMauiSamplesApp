using CommunityToolkit.Maui.Views;

namespace MyMauiSamplesApp;

public partial class PopupObject : Popup<object>
{
    public PopupObject()
    {
        InitializeComponent();
    }

    async void OnYesButtonClicked(object? sender, EventArgs e)
    {
        await CloseAsync(true);
    }

    async void OnNoButtonClicked(object? sender, EventArgs e)
    {
        await CloseAsync(false);
    }
}
