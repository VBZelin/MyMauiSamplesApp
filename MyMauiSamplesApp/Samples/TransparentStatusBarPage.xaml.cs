namespace MyMauiSamplesApp;

public partial class TransparentStatusBarPage : ContentPage
{
    public TransparentStatusBarPage()
    {
        InitializeComponent();
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}