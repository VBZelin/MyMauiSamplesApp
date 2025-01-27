using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MyMauiSamplesApp
{
    public partial class PreferenceItem(string key, string value) : ObservableObject
    {
        [ObservableProperty]
        private string _key = key;

        [ObservableProperty]
        private string _value = value;
    }

    public partial class PreferenceSamplePage : BasePage
    {
        public ObservableCollection<PreferenceItem> SavedPreferences { get; set; } = [];

        public PreferenceSamplePage()
        {
            InitializeComponent();
            BindingContext = this;
            ClearPreferences();
        }

        private void ClearPreferences()
        {
            Preferences.Default.Clear();
            SavedPreferences.Clear();
            SetSavePathLabel();
        }

        private void SetSavePathLabel()
        {
            string text = "Preferences are saved in the application's local storage.";

            if (DeviceInfo.Platform == DevicePlatform.iOS)
            {
                text = "Preferences are saved in UserDefaults.";
            }
            else if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                text = "Preferences are saved in SharedPreferences.";
            }
            else if (DeviceInfo.Platform == DevicePlatform.MacCatalyst)
            {
                text = "Preferences are saved in UserDefaults (Mac).";
            }
            else if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                text = "Preferences are saved in the local app data store.";
            }

            savePathLabel.Text = text;
        }

        private async void SavePreferenceClicked(object sender, EventArgs e)
        {
            var key = keyEntry.Text.Trim();
            var value = valueEntry.Text.Trim();

            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
            {
                await DisplayAlert("Error", "Key and value cannot be empty.", "OK");
                return;
            }

            Preferences.Default.Set(key, value);

            if (SavedPreferences.FirstOrDefault(p => p.Key == key) is { } existingItem)
            {
                existingItem.Value = value;
            }
            else
            {
                SavedPreferences.Add(new PreferenceItem(key, value));
            }

            keyEntry.Text = string.Empty;
            valueEntry.Text = string.Empty;
            await DisplayAlert("Saved", "Your preference has been saved.", "OK");
        }

        [RelayCommand]
        private async Task DeletePreferenceClicked(string key)
        {
            if (Preferences.Default.ContainsKey(key))
            {
                Preferences.Default.Remove(key);
                var item = SavedPreferences.FirstOrDefault(p => p.Key == key);
                if (item is not null)
                {
                    SavedPreferences.Remove(item);
                }
                await DisplayAlert("Deleted", $"Preference for {key} has been deleted.", "OK");
            }
        }

        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}