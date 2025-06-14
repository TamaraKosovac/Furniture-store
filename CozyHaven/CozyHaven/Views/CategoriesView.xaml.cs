using System.Windows;
using System.Windows.Controls;
using CozyHaven.ViewModels;

namespace CozyHaven.Views
{
    public partial class CategoriesView : UserControl
    {
        public CategoriesView()
        {
            InitializeComponent();
        }

        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            var addCategoryWindow = new AddCategoryView();
            addCategoryWindow.ShowDialog();

            if (DataContext is CategoriesViewModel vm)
            {
                vm.LoadCategories();
            }
        }
    }
}
