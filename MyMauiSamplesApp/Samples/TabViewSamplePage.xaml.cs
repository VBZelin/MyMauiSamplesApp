using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MyMauiSamplesApp;

public class TabViewItemModel
{
    public required string DisplayText { get; set; }
}

public partial class TabViewSamplePage : BasePage, INotifyPropertyChanged
{
    public ObservableCollection<TabViewItemModel> Items { get; set; } =
    [
        new TabViewItemModel { DisplayText = "Overview" },
        new TabViewItemModel { DisplayText = "Details" },
        new TabViewItemModel { DisplayText = "Settings" },
        new TabViewItemModel { DisplayText = "Feedback" }
    ];

    public TabViewSamplePage()
    {
        InitializeComponent();
    }

    private void TouchArea_Clicked(object sender, EventArgs e)
    {
        if (sender is not Element element)
            return;

        if (element.BindingContext is not TabViewItemModel item)
            return;

        int index = Items.IndexOf(item);
        if (index >= 0)
        {
            tabView.SelectedIndex = index;
        }
    }
}
