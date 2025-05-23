using CozyHaven.Models;
using System.Windows;
using System.Windows.Input;

namespace CozyHaven.Views
{
    public partial class ProductDetailsView : Window
    {
        public ProductDetailsView()
        {
            InitializeComponent();
        }

        public ProductDetailsView(Product product)
        {
            InitializeComponent();
            DataContext = product;
        }

        private void Close_Click(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                this.DragMove();
        }
    }
}
