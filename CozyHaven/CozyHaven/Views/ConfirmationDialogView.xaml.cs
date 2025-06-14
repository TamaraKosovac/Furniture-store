using System.Windows;
using System.Windows.Input;

namespace CozyHaven.Views
{
    public partial class ConfirmationDialogView : Window
    {
        public ConfirmationDialogView()
        {
            InitializeComponent();
            MessageText.Text = Application.Current.TryFindResource("LogoutConfirmation") as string ?? "";
        }

        public ConfirmationDialogView(string message)
        {
            InitializeComponent();
            MessageText.Text = message;
        }


        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void Yes_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
