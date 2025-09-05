using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Extensions;

namespace MyMauiSamplesApp;

public partial class PopupPage : BasePage
{
    public PopupPage()
    {
        InitializeComponent();
    }

    private async void OnOpenPopupClicked(object sender, EventArgs e)
    {
        var popup = new PopupObject();

        IPopupResult<object> result = await this.ShowPopupAsync<object>(popup, PopupOptions.Empty, CancellationToken.None);

        if (result.WasDismissedByTappingOutsideOfPopup)
        {
            return;
        }

        bool value = false;
        if (result.Result is bool b)
        {
            value = b;
        }
        else if (result.Result is string s && bool.TryParse(s, out var parsed))
        {
            value = parsed;
        }
        resultLabel.Text = $"Popup returned: {value}";
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}
