using CozyHaven.Models;
using CozyHaven.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CozyHaven.Views
{
    public partial class SettingsView : UserControl
    {
        public SettingsView(User user, bool showMyAccountButton = true)
        {
            InitializeComponent();
            DataContext = new SettingsViewModel(user, showMyAccountButton);
        }

        private void AdminAccount_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is SettingsViewModel vm)
            {
                var window = new AdminAccountView(vm);
                window.Owner = Window.GetWindow(this); 
                window.ShowDialog();
            }
        }

        private void ThemeComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            DisableSelectedThemeItem();
        }

        private void ThemeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DisableSelectedThemeItem();
        }

        private void DisableSelectedThemeItem()
        {
            if (ThemeComboBox.SelectedValue is string selectedTag)
            {
                foreach (ComboBoxItem item in ThemeComboBox.Items)
                {
                    if (item.Tag?.ToString() == selectedTag)
                    {
                        item.IsEnabled = false;
                        item.Foreground = Brushes.Gray;
                    }
                    else
                    {
                        item.IsEnabled = true;
                        item.ClearValue(ForegroundProperty);
                    }
                }
            }
        }
        private void LanguageComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            DisableSelectedLanguageItem();
        }

        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DisableSelectedLanguageItem();
        }

        private void DisableSelectedLanguageItem()
        {
            if (LanguageComboBox.SelectedValue is string selectedTag)
            {
                foreach (ComboBoxItem item in LanguageComboBox.Items)
                {
                    if (item.Tag?.ToString() == selectedTag)
                    {
                        item.IsEnabled = false;
                        item.Foreground = Brushes.Gray;
                    }
                    else
                    {
                        item.IsEnabled = true;
                        item.ClearValue(ForegroundProperty);
                    }
                }
            }
        }
    }
}
