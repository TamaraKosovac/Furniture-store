using CozyHaven.Models;
using CozyHaven.ViewModels;
using System.Windows;
namespace CozyHaven.Views
{
    public partial class EmployeeView : Window
    {
        private User _user;

        public EmployeeView(User user)
        {
            InitializeComponent();
            _user = user;
            ShowCategories(); 
            CategoriesButton.Tag = "Active"; 
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
            CategoriesButton.ClearValue(FrameworkElement.TagProperty);
            ProductsButton.ClearValue(FrameworkElement.TagProperty);
            IssueBillButton.ClearValue(FrameworkElement.TagProperty);
            BillsButton.ClearValue(FrameworkElement.TagProperty);
            SettingsButton.ClearValue(FrameworkElement.TagProperty);
            LogoutButton.ClearValue(FrameworkElement.TagProperty);
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new SettingsView(_user, false);
            ResetMenuButtonStates();
            SettingsButton.Tag = "Active";
        }

        private void CategoriesButton_Click(object sender, RoutedEventArgs e)
        {
            ShowCategories();
            ResetMenuButtonStates();
            CategoriesButton.Tag = "Active";
        }

        private void ShowCategories()
        {
            MainContent.Content = new CategoriesView();
        }

        private void ProductsButton_Click(object sender, RoutedEventArgs e)
        {
            ShowProducts();
            ResetMenuButtonStates();
            ProductsButton.Tag = "Active";
        }

        private void ShowProducts()
        {
            MainContent.Content = new ProductsView();
        }

        private void IssueBillButton_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new IssueBillView(_user); 
            ResetMenuButtonStates();
            IssueBillButton.Tag = "Active";
        }

        private void BillsButton_Click(object sender, RoutedEventArgs e)
        {
            CleanupUnissuedBill(); 
            MainContent.Content = new BillsView(); 
            ResetMenuButtonStates();
            BillsButton.Tag = "Active";
        }

        private void CleanupUnissuedBill()
        {
            if (MainContent.Content is IssueBillView billsView &&
                billsView.DataContext is ProductsViewModel vm)
            {
                vm.RevertUnpaidItems();
            }
        }
    }
}
