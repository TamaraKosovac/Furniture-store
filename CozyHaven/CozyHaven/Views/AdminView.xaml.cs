using CozyHaven.Models;
using System.Windows;


namespace CozyHaven.Views
{
    public partial class AdminView : Window
    {
        private User _user;
        public AdminView(User user)
        {
            InitializeComponent();
            _user = user;
            MainContent.Content = new EmployeesView();
            ResetMenuButtonStates();
            EmployeesButton.Tag = "Active";
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new SettingsView(_user, true); 
            ResetMenuButtonStates();
            SettingsButton.Tag = "Active";
        }


        private void Employees_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new EmployeesView(); 
            ResetMenuButtonStates();
            EmployeesButton.Tag = "Active";
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            var confirmDialog = new ConfirmationDialogView();
            bool? result = confirmDialog.ShowDialog();

            if (result == true)
            {
                var loginWindow = new LoginView();
                loginWindow.Show();
                this.Close();
            }
            else
            {
                ResetMenuButtonStates();
                LogoutButton.Tag = "Active";
            }
        }

        private void ResetMenuButtonStates()
        {
            SettingsButton.ClearValue(FrameworkElement.TagProperty);
            EmployeesButton.ClearValue(FrameworkElement.TagProperty);
            LogoutButton.ClearValue(FrameworkElement.TagProperty);
        }

    }
}
