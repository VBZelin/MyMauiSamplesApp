namespace MyMauiSamplesApp;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	private async void OnPreferenceClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(PreferenceSamplePage));
	}

	private async void OnRadCollectionViewClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(RadCollectionViewPage));
	}
}

