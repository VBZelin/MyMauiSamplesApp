using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System.Linq;
using CommunityToolkit.Mvvm.Input;

namespace MyMauiSamplesApp
{
    public class PreferenceItem(string key, string value)
    {
        public string Key { get; set; } = key;
        public string Value { get; set; } = value;
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
        }

        private async void SavePreferenceClicked(object sender, EventArgs e)
        {
            var key = keyEntry.Text;
            var value = valueEntry.Text;
            if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
            {
                Preferences.Default.Set(key, value);
                SavedPreferences.Add(new PreferenceItem(key, value));
                keyEntry.Text = string.Empty;
                valueEntry.Text = string.Empty;
                await DisplayAlert("Saved", "Your preference has been saved.", "OK");
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