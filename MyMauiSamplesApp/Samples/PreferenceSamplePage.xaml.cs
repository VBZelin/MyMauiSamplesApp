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

    public partial class PreferenceSamplePage : ContentPage
    {
        public ObservableCollection<PreferenceItem> SavedPreferences { get; set; } = [];

        public PreferenceSamplePage()
        {
            InitializeComponent();
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
            var key = keyEntry.Text;
            var value = valueEntry.Text;
            if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
            {
                Preferences.Default.Set(key, value);

                var existingItem = SavedPreferences.FirstOrDefault(p => p.Key == key);
                if (existingItem is not null)
                {
                    existingItem.Value = value;
                }
                else
                {
                    SavedPreferences.Add(new PreferenceItem(key, value));
                }
            }
            else
            {
                await DisplayAlert("Error", "Key or value cannot be empty.", "OK");
            }
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
    }
}