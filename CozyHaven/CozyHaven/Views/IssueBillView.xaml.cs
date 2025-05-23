using CozyHaven.Models;
using CozyHaven.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace CozyHaven.Views
{
    public partial class IssueBillView : UserControl
    {
        private readonly ProductsViewModel _viewModel;

        public IssueBillView(User loggedInUser)
        {
            InitializeComponent();
            _viewModel = new ProductsViewModel(loggedInUser);
            DataContext = _viewModel;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            _viewModel.RevertUnpaidItems();
        }
    }
}
