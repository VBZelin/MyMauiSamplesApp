using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Extensions;

namespace MyMauiSamplesApp;

public partial class PopupPage : ContentPage
{
    public PopupPage()
    {
        InitializeComponent();
    }

    private async void OnOpenPopupClicked(object sender, EventArgs e)
    {
        var popup = new PopupBool();

        IPopupResult<bool> result = await this.ShowPopupAsync<bool>(popup, PopupOptions.Empty, CancellationToken.None);

        if (result.WasDismissedByTappingOutsideOfPopup)
        {
            return;
        }

        resultLabel.Text = $"Popup returned: {result.Result}";
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}
