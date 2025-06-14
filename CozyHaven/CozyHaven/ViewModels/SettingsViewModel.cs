using CozyHaven.Models;
using CozyHaven.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using CozyHaven.Helpers;
using System.Text.RegularExpressions;
using CozyHaven.Views;

namespace CozyHaven.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        private readonly User _currentUser;
        public Action? CloseAction { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName!));

        private bool _showMyAccountButton;
        public bool ShowMyAccountButton
        {
            get => _showMyAccountButton;
            set { _showMyAccountButton = value; OnPropertyChanged(); }
        }

        public ObservableCollection<string> AvailableThemes { get; set; }
        public ObservableCollection<string> AvailableLanguages { get; set; }

        private string _salary = string.Empty;
        public string Salary
        {
            get => _salary;
            set { _salary = value; OnPropertyChanged(); }
        }

        private string _position = string.Empty;
        public string Position
        {
            get => _position;
            set { _position = value; OnPropertyChanged(); }
        }

        private string? _selectedTheme;
        public string SelectedTheme
        {
            get => _selectedTheme!;
            set
            {
                if (_selectedTheme != value)
                {
                    _selectedTheme = value;
                    OnPropertyChanged();
                    ApplyTheme(value);
                    IsThemeComboBoxVisible = false;
                }
            }
        }

        private string? _selectedLanguage;
        public string SelectedLanguage
        {
            get => _selectedLanguage!;
            set
            {
                if (_selectedLanguage != value)
                {
                    _selectedLanguage = value;
                    OnPropertyChanged();
                    ApplyLanguage(value);
                    IsLanguageComboBoxVisible = false;
                }
            }
        }

        private bool _isThemeComboBoxVisible;
        public bool IsThemeComboBoxVisible
        {
            get => _isThemeComboBoxVisible;
            set { _isThemeComboBoxVisible = value; OnPropertyChanged(); }
        }

        private bool _isLanguageComboBoxVisible;
        public bool IsLanguageComboBoxVisible
        {
            get => _isLanguageComboBoxVisible;
            set { _isLanguageComboBoxVisible = value; OnPropertyChanged(); }
        }

        private bool _isAdminFormVisible;
        public bool IsAdminFormVisible
        {
            get => _isAdminFormVisible;
            set { _isAdminFormVisible = value; OnPropertyChanged(); }
        }

        private string _firstName = string.Empty;
        public string FirstName { get => _firstName; set { _firstName = value; OnPropertyChanged(); } }

        private string _lastName = string.Empty;
        public string LastName { get => _lastName; set { _lastName = value; OnPropertyChanged(); } }

        private string _username = string.Empty;
        public string Username { get => _username; set { _username = value; OnPropertyChanged(); } }

        private string _storedPassword = string.Empty;
        private string _newPassword = string.Empty;
        public string NewPassword
        {
            get => _newPassword;
            set { _newPassword = value; OnPropertyChanged(); }
        }

        private string _phoneNumber = string.Empty;
        public string PhoneNumber { get => _phoneNumber; set { _phoneNumber = value; OnPropertyChanged(); } }

        private string _address = string.Empty;
        public string Address { get => _address; set { _address = value; OnPropertyChanged(); } }

        private string _shift = string.Empty;
        public string Shift { get => _shift; set { _shift = value; OnPropertyChanged(); } }

        public ICommand ToggleThemeVisibilityCommand { get; }
        public ICommand ToggleLanguageVisibilityCommand { get; }
        public ICommand SaveAdminCommand => new RelayCommand(_ => SaveAdminChanges());

        public SettingsViewModel(User user, bool showMyAccountButton = true)
        {
            _currentUser = user;
            ShowMyAccountButton = showMyAccountButton;

            AvailableThemes = new ObservableCollection<string> { "Light", "Gray", "Dark" };
            AvailableLanguages = new ObservableCollection<string> { "en", "sr" };

            SelectedTheme = SessionManager.CurrentTheme
                            ?? _currentUser.PreferredTheme
                            ?? "Light";

            SelectedLanguage = SessionManager.CurrentLanguage
                               ?? _currentUser.PreferredLanguage
                               ?? "en";

            ToggleThemeVisibilityCommand = new RelayCommand(_ =>
                IsThemeComboBoxVisible = !IsThemeComboBoxVisible);

            ToggleLanguageVisibilityCommand = new RelayCommand(_ =>
                IsLanguageComboBoxVisible = !IsLanguageComboBoxVisible);

            using var db = new AppDbContext();
            var admin = db.Users.FirstOrDefault(u => u.Role == "admin");
            if (admin != null)
            {
                FirstName = admin.FirstName;
                LastName = admin.LastName;
                Username = admin.Username;
                _storedPassword = admin.Password;
                PhoneNumber = admin.PhoneNumber;
                Address = admin.Address;
                Shift = admin.Shift;
                Position = admin.Position;
                Salary = admin.Salary.ToString("F2");
            }
        }

        private void SaveAdminChanges()
        {
            if (string.IsNullOrWhiteSpace(FirstName) ||
                string.IsNullOrWhiteSpace(LastName) ||
                string.IsNullOrWhiteSpace(Username) ||
                string.IsNullOrWhiteSpace(PhoneNumber) ||
                string.IsNullOrWhiteSpace(Address) ||
                string.IsNullOrWhiteSpace(Shift) ||
                string.IsNullOrWhiteSpace(Position) ||
                string.IsNullOrWhiteSpace(Salary))
            {
                ShowMessageByKey("EmptyFields");
                return;
            }

            if (!string.IsNullOrWhiteSpace(NewPassword) && !IsPasswordStrong(NewPassword))
            {
                ShowMessageByKey("InvalidPassword");
                return;
            }

            if (!Regex.IsMatch(PhoneNumber, @"^\+?[0-9\s\-]{6,15}$"))
            {
                ShowMessageByKey("InvalidPhone");
                return;
            }

            string normalizedSalary = Salary.Replace(',', '.');
            if (!decimal.TryParse(normalizedSalary, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var salaryValue))
            {
                ShowMessageByKey("InvalidSalary");
                return;
            }

            using var context = new AppDbContext();
            var admin = context.Users.FirstOrDefault(u => u.Role == "admin");

            if (admin != null)
            {
                admin.FirstName = FirstName;
                admin.LastName = LastName;
                admin.Username = Username;
                admin.Password = string.IsNullOrWhiteSpace(NewPassword)
                    ? _storedPassword
                    : PasswordHelper.HashPassword(NewPassword);
                admin.PhoneNumber = PhoneNumber;
                admin.Address = Address;
                admin.Shift = Shift;
                admin.Position = Position;
                admin.Salary = Math.Round(salaryValue, 2);
                context.SaveChanges();
            }

            ShowMessageByKey("SaveSuccessAdmin");
            CloseAction?.Invoke();
        }

        private void ApplyTheme(string themeName)
        {
            SessionManager.CurrentTheme = themeName;

            string themePath = themeName switch
            {
                "Light" => "Themes/LightTheme.xaml",
                "Gray" => "Themes/GrayTheme.xaml",
                "Dark" => "Themes/DarkTheme.xaml",
                _ => "Themes/LightTheme.xaml"
            };

            var themeDict = new ResourceDictionary { Source = new Uri(themePath, UriKind.Relative) };
            var stylesDict = new ResourceDictionary { Source = new Uri("Resources/Styles.xaml", UriKind.Relative) };
            var materialDesignDefaults = new ResourceDictionary
            {
                Source = new Uri("pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesign3.Defaults.xaml")
            };

            for (int i = Application.Current.Resources.MergedDictionaries.Count - 1; i >= 0; i--)
            {
                var md = Application.Current.Resources.MergedDictionaries[i];
                if (md.Source != null &&
                    (md.Source.OriginalString.Contains("Theme") ||
                     md.Source.OriginalString.Contains("Styles.xaml") ||
                     md.Source.OriginalString.Contains("MaterialDesign3.Defaults.xaml")))
                {
                    Application.Current.Resources.MergedDictionaries.Remove(md);
                }
            }

            Application.Current.Resources.MergedDictionaries.Add(materialDesignDefaults);
            Application.Current.Resources.MergedDictionaries.Add(themeDict);
            Application.Current.Resources.MergedDictionaries.Add(stylesDict);

            using var context = new AppDbContext();
            var userInDb = context.Users.FirstOrDefault(u => u.Id == _currentUser.Id);
            if (userInDb != null)
            {
                userInDb.PreferredTheme = themeName;
                context.SaveChanges();
            }
        }

        private void ApplyLanguage(string langCode)
        {
            SessionManager.CurrentLanguage = langCode;

            string dictionaryPath = langCode == "sr"
                ? "Languages/SerbianDictionary.xaml"
                : "Languages/EnglishDictionary.xaml";

            var newDict = new ResourceDictionary
            {
                Source = new Uri(dictionaryPath, UriKind.Relative)
            };

            for (int i = Application.Current.Resources.MergedDictionaries.Count - 1; i >= 0; i--)
            {
                var md = Application.Current.Resources.MergedDictionaries[i];
                if (md.Source != null && md.Source.OriginalString.Contains("Dictionary"))
                {
                    Application.Current.Resources.MergedDictionaries.Remove(md);
                }
            }

            Application.Current.Resources.MergedDictionaries.Add(newDict);

            using var context = new AppDbContext();
            var userInDb = context.Users.FirstOrDefault(u => u.Id == _currentUser.Id);
            if (userInDb != null)
            {
                userInDb.PreferredLanguage = langCode;
                context.SaveChanges();
            }
        }

        private void ShowMessage(string message)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var messageBox = new MessageBoxView(message);
                messageBox.ShowDialog();
            });
        }

        private void ShowMessageByKey(string resourceKey)
        {
            if (Application.Current.Resources[resourceKey] is string message)
            {
                ShowMessage(message);
            }
            else
            {
                ShowMessage($"[Nedostaje prijevod za ključ: {resourceKey}]");
            }
        }

        private bool IsPasswordStrong(string password)
        {
            if (string.IsNullOrWhiteSpace(password)) return false;

            return password.Length >= 8 &&
                   Regex.IsMatch(password, "[A-Z]") &&
                   Regex.IsMatch(password, "[a-z]") &&
                   Regex.IsMatch(password, "[0-9]") &&
                   Regex.IsMatch(password, @"[\W_]");
        }
    }
}
