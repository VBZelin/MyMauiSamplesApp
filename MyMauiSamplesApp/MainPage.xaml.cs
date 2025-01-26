namespace MyMauiSamplesApp;

public partial class MainPage : BasePage
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

	private async void OnTransparentStatusBarClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(TransparentStatusBarPage));
	}
}

