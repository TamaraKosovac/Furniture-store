using CozyHaven.Data;
using CozyHaven.Helpers;
using CozyHaven.Models;
using CozyHaven.Views;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace CozyHaven.ViewModels
{
    public class AddCategoryViewModel : INotifyPropertyChanged
    {
        private readonly Category? _existingCategory;
        private string _categoryName = string.Empty;

        public string CategoryName
        {
            get => _categoryName;
            set
            {
                if (_categoryName != value)
                {
                    _categoryName = value;
                    OnPropertyChanged();
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public ICommand SaveCategoryCommand { get; }
        public Action? CloseAction { get; set; }

        public AddCategoryViewModel()
        {
            SaveCategoryCommand = new RelayCommand(SaveCategory, CanSave);
        }

        public AddCategoryViewModel(Category categoryToEdit)
        {
            _existingCategory = categoryToEdit;
            CategoryName = categoryToEdit.Name;
            SaveCategoryCommand = new RelayCommand(UpdateCategory, CanSave);
        }

        private bool CanSave(object obj) => !string.IsNullOrWhiteSpace(CategoryName);

        private void SaveCategory(object obj)
        {
            using var context = new AppDbContext();

            bool exists = context.Categories
                .Any(c => c.Name.ToLower() == CategoryName.Trim().ToLower());

            if (exists)
            {
                ShowMessage("CategoryExists");
                return;
            }

            var newCategory = new Category
            {
                Name = CategoryName.Trim()
            };

            context.Categories.Add(newCategory);
            context.SaveChanges();

            ShowMessage("CategoryAdded");
            CloseAction?.Invoke();
        }

        private void UpdateCategory(object obj)
        {
            using var context = new AppDbContext();
            var category = context.Categories.FirstOrDefault(c => c.Id == _existingCategory!.Id);

            if (category != null)
            {
                category.Name = CategoryName.Trim();
                context.SaveChanges();
                ShowMessage("CategoryUpdated");
            }

            CloseAction?.Invoke();
        }

        private void ShowMessage(string resourceKey)
        {
            string message = Application.Current.TryFindResource(resourceKey) as string ?? resourceKey;
            new MessageBoxView(message).ShowDialog();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
