using CozyHaven.Models;
using CozyHaven.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace CozyHaven.Views
{
    public partial class AddCategoryView : Window
    {
        public AddCategoryView()
        {
            InitializeComponent();
            var vm = new AddCategoryViewModel();
            vm.CloseAction = Close;
            DataContext = vm;
        }

        public AddCategoryView(Category categoryToEdit)
        {
            InitializeComponent();
            var vm = new AddCategoryViewModel(categoryToEdit);
            vm.CloseAction = Close;
            DataContext = vm;
        }


        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
