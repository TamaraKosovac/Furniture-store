using System.Windows;
using System.Windows.Input;


namespace CozyHaven.Views
{
    public partial class BillMessageBox : Window
    {
        public BillMessageBox()
        {
            InitializeComponent();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }
    }
}
