using CozyHaven.ViewModels;
using System.Windows;
using System.Windows.Input;


namespace CozyHaven.Views
{
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void WindowMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel vm)
            {
                vm.Username = UsernameInput.Text;

                if (!vm.IsPasswordVisible)
                    vm.Password = PasswordInput.Password;
                else
                    vm.Password = VisiblePasswordInput.Text;
            }
        }

        private void PasswordInput_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel vm)
            {
                vm.Password = PasswordInput.Password;
            }
        }
        private void TogglePasswordVisibility_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel vm)
                vm.IsPasswordVisible = !vm.IsPasswordVisible;
        }
    }
}
