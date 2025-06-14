using CozyHaven.Data;
using CozyHaven.Helpers;
using CozyHaven.Models;
using CozyHaven.Views;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace CozyHaven.ViewModels
{
    public class EmployeeListViewModel : INotifyPropertyChanged
    {
        private string _searchQuery = string.Empty;
        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                if (_searchQuery != value)
                {
                    _searchQuery = value;
                    OnPropertyChanged();
                    ApplySearchFilter();
                }
            }
        }

        private ObservableCollection<User> _allEmployees = new();
        public ObservableCollection<User> FilteredEmployees { get; set; } = new();

        public ICommand DeleteEmployeeCommand { get; }
        public ICommand UpdateEmployeeCommand { get; }

        public EmployeeListViewModel()
        {
            DeleteEmployeeCommand = new RelayCommand(DeleteEmployee);
            UpdateEmployeeCommand = new RelayCommand(UpdateEmployee);
            LoadEmployees();
        }

        private void LoadEmployees()
        {
            using var context = new AppDbContext();

            var employeesFromDb = context.Users
                .Include(u => u.Department)
                .Where(u => u.Role == "employee" && !u.IsDeleted)
                .ToList();

            _allEmployees.Clear();
            foreach (var employee in employeesFromDb)
                _allEmployees.Add(employee);

            ApplySearchFilter();
        }

        private void ApplySearchFilter()
        {
            FilteredEmployees.Clear();

            var filtered = string.IsNullOrWhiteSpace(SearchQuery)
                ? _allEmployees
                : _allEmployees.Where(e => e.Username.Contains(SearchQuery, System.StringComparison.OrdinalIgnoreCase));

            foreach (var emp in filtered)
                FilteredEmployees.Add(emp);
        }

        private void DeleteEmployee(object obj)
        {
            if (obj is User user)
            {
                var message = Application.Current.TryFindResource("DeleteEmployeeConfirmation") as string
                              ?? "Are you sure you want to delete this employee?";
                var confirmationDialog = new ConfirmationDialogView(message);
                var result = confirmationDialog.ShowDialog();

                if (result == true)
                {
                    using var context = new AppDbContext();
                    var toDelete = context.Users.FirstOrDefault(u => u.Id == user.Id);
                    if (toDelete != null)
                    {
                        toDelete.IsDeleted = true;
                        context.SaveChanges();
                        _allEmployees.Remove(user);
                        FilteredEmployees.Remove(user);

                        var successMessage = Application.Current.TryFindResource("EmployeeDeleted") as string
                                             ?? "Employee successfully deleted.";
                        var messageBox = new MessageBoxView(successMessage);
                        messageBox.ShowDialog();
                    }
                }
            }
        }


        private void UpdateEmployee(object obj)
        {
            if (obj is User user)
            {
                var viewModel = new EmployeeViewModel();
                viewModel.LoadUser(user);
                viewModel.CloseAction = () =>
                {
                    LoadEmployees();
                    Application.Current.Windows
                        .OfType<Window>()
                        .FirstOrDefault(w => w is EmployeeAccountView)
                        ?.Close();
                };

                var window = new EmployeeAccountView
                {
                    DataContext = viewModel
                };

                window.ShowDialog();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public void RefreshEmployees()
        {
            LoadEmployees();
        }
    }
}
