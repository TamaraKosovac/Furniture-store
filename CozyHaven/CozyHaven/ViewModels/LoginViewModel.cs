using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using CozyHaven.Data;
using CozyHaven.Helpers;
using CozyHaven.Views;

namespace CozyHaven.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string _username = string.Empty;
        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); }
        }

        private string _password = string.Empty;
        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        private bool _isPasswordVisible;
        public bool IsPasswordVisible
        {
            get => _isPasswordVisible;
            set { _isPasswordVisible = value; OnPropertyChanged(); }
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(_ => Login());
        }

        private void Login()
        {
            using var context = new AppDbContext();

            if (string.IsNullOrWhiteSpace(Username))
            {
                var messageBox = new MessageBoxView("Unesite korisničko ime.");
                messageBox.ShowDialog();
                return;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                var messageBox = new MessageBoxView("Unesite lozinku.");
                messageBox.ShowDialog();
                return;
            }

            var user = context.Users
                .AsEnumerable()
                .FirstOrDefault(u => u.Username.Equals(Username, StringComparison.Ordinal));

            if (user == null)
            {
                var message = Application.Current.TryFindResource("UserNotRegistered") as string;
                var messageBox = new MessageBoxView(message ?? "Korisnik nije registrovan.");
                messageBox.ShowDialog();
                return;
            }

            var hashed = PasswordHelper.HashPassword(Password);
            if (user.Password != hashed)
            {
                var message = Application.Current.TryFindResource("InvalidLoginPassword") as string;
                var messageBox = new MessageBoxView(message ?? "Pogrešna lozinka.");
                messageBox.ShowDialog();
                return;
            }

            ApplyUserPreferences(user.PreferredTheme, user.PreferredLanguage);

            Window nextWindow = user.Role == "admin"
                ? new AdminView(user)
                : new EmployeeView(user);

            nextWindow.Show();
            foreach (Window win in Application.Current.Windows)
            {
                if (win is Window && win.Title == "CozyHaven")
                {
                    win.Close();
                    break;
                }
            }
        }

        private void ApplyUserPreferences(string themeName, string langCode)
        {
            string themePath = themeName switch
            {
                "Light" => "Themes/LightTheme.xaml",
                "Gray" => "Themes/GrayTheme.xaml",
                "Dark" => "Themes/DarkTheme.xaml",
                _ => "Themes/LightTheme.xaml"
            };

            string langPath = langCode == "sr"
                ? "Languages/SerbianDictionary.xaml"
                : "Languages/EnglishDictionary.xaml";

            var themeDict = new ResourceDictionary { Source = new Uri(themePath, UriKind.Relative) };
            var stylesDict = new ResourceDictionary { Source = new Uri("Resources/Styles.xaml", UriKind.Relative) };
            var langDict = new ResourceDictionary { Source = new Uri(langPath, UriKind.Relative) };
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
                     md.Source.OriginalString.Contains("MaterialDesign3.Defaults.xaml") ||
                     md.Source.OriginalString.Contains("Languages/")))
                {
                    Application.Current.Resources.MergedDictionaries.RemoveAt(i);
                }
            }

            Application.Current.Resources.MergedDictionaries.Add(materialDesignDefaults);
            Application.Current.Resources.MergedDictionaries.Add(themeDict);
            Application.Current.Resources.MergedDictionaries.Add(stylesDict);
            Application.Current.Resources.MergedDictionaries.Add(langDict);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
