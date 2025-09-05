using CommunityToolkit.Maui.Views;

namespace MyMauiSamplesApp;

public partial class PopupBool : Popup<bool>
{
    public PopupBool()
    {
        InitializeComponent();
    }

    void OnYesClicked(object sender, EventArgs e) => CloseAsync(true);
    void OnNoClicked(object sender, EventArgs e) => CloseAsync(false);
}