using CozyHaven.Data;
using CozyHaven.Helpers;
using CozyHaven.Models;
using CozyHaven.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace CozyHaven.ViewModels
{
    public class EmployeeViewModel : INotifyPropertyChanged
    {
        private readonly User _user = new();
        private int _editingUserId = 0;
        private bool _isEditing = false;
        private bool _isPasswordChanged = false;
        public Action? CloseAction { get; set; }

        public EmployeeViewModel()
        {
            using var context = new AppDbContext();
            Departments = new ObservableCollection<Department>(context.Departments.ToList());
            OnPropertyChanged(nameof(Departments));
        }


        public string FirstName
        {
            get => _user.FirstName;
            set { _user.FirstName = value; OnPropertyChanged(); }
        }

        public string LastName
        {
            get => _user.LastName;
            set { _user.LastName = value; OnPropertyChanged(); }
        }

        public string Username
        {
            get => _user.Username;
            set { _user.Username = value; OnPropertyChanged(); }
        }

        private string _password = string.Empty;
        public string Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
                    _isPasswordChanged = true;
                    OnPropertyChanged();
                }
            }
        }

        public string PhoneNumber
        {
            get => _user.PhoneNumber;
            set { _user.PhoneNumber = value; OnPropertyChanged(); }
        }

        public string Address
        {
            get => _user.Address;
            set { _user.Address = value; OnPropertyChanged(); }
        }

        public string Shift
        {
            get => _user.Shift;
            set { _user.Shift = value; OnPropertyChanged(); }
        }

        private string _position = string.Empty;
        public string Position
        {
            get => _position;
            set
            {
                _position = value;
                _user.Position = value;
                OnPropertyChanged();
            }
        }

        private string _salary = string.Empty;
        public string Salary
        {
            get => _salary;
            set
            {
                _salary = value;
                OnPropertyChanged();
            }
        }

        private string? _employmentDate;
        public string? EmploymentDate
        {
            get => _employmentDate;
            set
            {
                _employmentDate = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _employmentDateSelected;
        public DateTime? EmploymentDateSelected
        {
            get => _employmentDateSelected;
            set
            {
                _employmentDateSelected = value;
                OnPropertyChanged();

                if (value.HasValue)
                {
                    EmploymentDate = value.Value.ToString("dd.MM.yyyy");
                    EmploymentDateRaw = value.Value.Date;
                }
            }
        }

        public DateTime EmploymentDateRaw
        {
            get => _user.EmploymentDate;
            set
            {
                _user.EmploymentDate = value.Date;
                OnPropertyChanged();
            }
        }

        private string? _profilePicturePath;
        public string? ProfilePicturePath
        {
            get => _profilePicturePath;
            set
            {
                _profilePicturePath = value;
                _user.ProfilePicturePath = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Department> Departments { get; set; } = new();
        private Department? _selectedDepartment;

        public Department? SelectedDepartment
        {
            get => _selectedDepartment;
            set
            {
                _selectedDepartment = value;
                if (value != null)
                    _user.DepartmentId = value.Id;
                OnPropertyChanged();
            }
        }


        public ICommand SaveAdminCommand => new RelayCommand(_ => SaveEmployee());

        public void LoadUser(User user)
        {
            _editingUserId = user.Id;
            _isEditing = true;
            _isPasswordChanged = false;

            FirstName = user.FirstName;
            LastName = user.LastName;
            Username = user.Username;
            Password = ""; 
            Position = user.Position;
            PhoneNumber = user.PhoneNumber;
            Address = user.Address;
            Shift = user.Shift;
            Salary = user.Salary.ToString("F2");
            EmploymentDateRaw = user.EmploymentDate;
            EmploymentDate = user.EmploymentDate.ToString("dd.MM.yyyy");
            EmploymentDateSelected = user.EmploymentDate;
            ProfilePicturePath = user.ProfilePicturePath;

            using var context = new AppDbContext();
            Departments = new ObservableCollection<Department>(context.Departments.ToList());
            SelectedDepartment = Departments.FirstOrDefault(d => d.Id == user.DepartmentId);
        }

        private void SaveEmployee()
        {
            if (string.IsNullOrWhiteSpace(FirstName) ||
                string.IsNullOrWhiteSpace(LastName) ||
                string.IsNullOrWhiteSpace(Username) ||
                string.IsNullOrWhiteSpace(PhoneNumber) ||
                string.IsNullOrWhiteSpace(Address) ||
                string.IsNullOrWhiteSpace(Shift) ||
                string.IsNullOrWhiteSpace(Position) ||
                string.IsNullOrWhiteSpace(Salary) ||
                EmploymentDateRaw == default ||
                SelectedDepartment == null)
            {
                ShowMessageByKey("EmptyFields");
                return;
            }

            if (!_isEditing && string.IsNullOrWhiteSpace(Password))
            {
                ShowMessageByKey("EmptyFields");
                return;
            }

            if (_isPasswordChanged || !_isEditing)
            {
                if (!IsPasswordStrong(Password))
                {
                    ShowMessageByKey("InvalidPassword");
                    return;
                }
            }

            if (!Regex.IsMatch(PhoneNumber, @"^\+?[0-9\s\-]{6,15}$"))
            {
                ShowMessageByKey("InvalidPhone");
                return;
            }

            string normalizedSalary = Salary.Replace(',', '.');
            if (!decimal.TryParse(normalizedSalary, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var salaryValue))
            {
                ShowMessageByKey("InvalidSalary");
                return;
            }

            using var context = new AppDbContext();

            var user = _editingUserId == 0
                ? new User()
                : context.Users.FirstOrDefault(u => u.Id == _editingUserId);

            if (user == null)
            {
                ShowMessage("Employee not found.");
                return;
            }

            if (_editingUserId == 0 && context.Users.Any(u => u.Username == Username))
            {
                ShowMessageByKey("UsernameExists");
                return;
            }

            user.FirstName = FirstName;
            user.LastName = LastName;
            user.Username = Username;

            if (_isPasswordChanged || !_isEditing)
                user.Password = PasswordHelper.HashPassword(Password);

            user.Position = Position;
            user.PhoneNumber = PhoneNumber;
            user.Address = Address;
            user.Shift = Shift;
            user.Salary = Math.Round(salaryValue, 2);
            user.EmploymentDate = EmploymentDateRaw;
            user.ProfilePicturePath = ProfilePicturePath;
            user.DepartmentId = SelectedDepartment.Id;
            user.Role = "employee";
            user.PreferredLanguage = "en";
            user.PreferredTheme = "Light";

            if (_editingUserId == 0)
                context.Users.Add(user);

            context.SaveChanges();
            ShowMessageByKey("SaveSuccess");
            CloseAction?.Invoke();
        }

        private bool IsPasswordStrong(string password)
        {
            return password.Length >= 8 &&
                   Regex.IsMatch(password, @"[A-Z]") &&
                   Regex.IsMatch(password, @"[a-z]") &&
                   Regex.IsMatch(password, @"[0-9]") &&
                   Regex.IsMatch(password, @"[\W_]");
        }

        private void ShowMessage(string message)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var messageBox = new MessageBoxView(message);
                messageBox.ShowDialog();
            });
        }

        private void ShowMessageByKey(string resourceKey)
        {
            if (Application.Current.Resources[resourceKey] is string message)
                ShowMessage(message);
            else
                ShowMessage($"[Nedostaje prijevod za ključ: {resourceKey}]");
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null!) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
