using Android.Views;

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

    protected override void OnAppearing()
    {
        base.OnAppearing();
        MakeStatusBarTransparent();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        ResetStatusBar();
    }

    private void MakeStatusBarTransparent()
    {

    }

    private void ResetStatusBar()
    {

    }
}