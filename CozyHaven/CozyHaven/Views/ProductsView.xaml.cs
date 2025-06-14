using System.Windows;
using System.Windows.Controls;

namespace CozyHaven.Views
{
    public partial class ProductsView : UserControl
    {
        public ProductsView()
        {
            InitializeComponent();
        }

        private void OpenAddProductWindow_Click(object sender, RoutedEventArgs e)
        {
            var window = new AddProductView();
            window.Owner = Window.GetWindow(this);
            window.ShowDialog();

            if (DataContext is ViewModels.ProductsViewModel vm)
            {
                vm.LoadProducts(); 
            }
        }
    }
}
