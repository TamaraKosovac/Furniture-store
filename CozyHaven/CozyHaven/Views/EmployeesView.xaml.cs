using CozyHaven.ViewModels;
using System.Windows;
using System.Windows.Controls;


namespace CozyHaven.Views
{
    public partial class EmployeesView : UserControl
    {
        private readonly EmployeeListViewModel _viewModel = new EmployeeListViewModel();

        public EmployeesView()
        {
            InitializeComponent();
            DataContext = _viewModel;
        }

        private void OpenEmployeeForm_Click(object sender, RoutedEventArgs e)
        {
            var window = new EmployeeAccountView();
            window.Owner = Window.GetWindow(this);
            window.ShowDialog();
            _viewModel.RefreshEmployees();
        }

    }
}
