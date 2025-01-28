namespace MyMauiSamplesApp;

public partial class SideDrawerPage : BasePage
{
    public SideDrawerPage()
    {
        InitializeComponent();
    }

    private void OnShowBottomSheetClicked(object sender, EventArgs e)
    {
        bottomSheet?.Open();
    }
}
