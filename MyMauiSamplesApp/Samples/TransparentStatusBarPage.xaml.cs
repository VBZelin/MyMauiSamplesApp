namespace MyMauiSamplesApp;

public partial class TransparentStatusBarPage : BasePage
{
    public TransparentStatusBarPage()
    {
        InitializeComponent();
        BindingContext = new TransparentStatusBarViewModel { Color = TransparentStatusBarPage.GetPrimaryColor() };
    }

    private static Color GetPrimaryColor()
    {
        return Application.Current?.Resources.TryGetValue("Primary", out var primaryColor) == true
            ? (Color)primaryColor
            : Colors.Transparent;
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}