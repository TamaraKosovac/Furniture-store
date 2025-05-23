using CozyHaven.Data;
using CozyHaven.Helpers;
using CozyHaven.Models;
using CozyHaven.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace CozyHaven.ViewModels
{
    public class CategoriesViewModel : INotifyPropertyChanged
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

        private ObservableCollection<Category> _allCategories = new();
        public ObservableCollection<Category> FilteredCategories { get; set; } = new();

        public ICommand UpdateCategoryCommand { get; }
        public ICommand DeleteCategoryCommand { get; }

        public CategoriesViewModel()
        {
            UpdateCategoryCommand = new RelayCommand(UpdateCategory);
            DeleteCategoryCommand = new RelayCommand(DeleteCategory);
            LoadCategories();
        }

        public void LoadCategories()
        {
            using var context = new AppDbContext();
            var categories = context.Categories.ToList();

            _allCategories = new ObservableCollection<Category>(categories);
            ApplySearchFilter();
        }

        private void ApplySearchFilter()
        {
            FilteredCategories.Clear();

            var filtered = string.IsNullOrWhiteSpace(SearchQuery)
                ? _allCategories
                : _allCategories.Where(c => c.Name.ToLower().Contains(SearchQuery.ToLower()));

            foreach (var cat in filtered)
                FilteredCategories.Add(cat);
        }

        private void UpdateCategory(object obj)
        {
            if (obj is Category category)
            {
                var window = new AddCategoryView(category);
                window.ShowDialog();
                LoadCategories();
            }
        }

        private void DeleteCategory(object obj)
        {
            if (obj is Category category)
            {
                string message = Application.Current.TryFindResource("CategoryDeleteMessage") as string
                                 ?? "Are you sure you want to delete?";

                var confirm = new ConfirmationDialogView(message);
                bool? result = confirm.ShowDialog();

                if (result == true)
                {
                    using var context = new AppDbContext();
                    var entity = context.Categories.FirstOrDefault(c => c.Id == category.Id);
                    if (entity != null)
                    {
                        context.Categories.Remove(entity);
                        context.SaveChanges();

                        new MessageBoxView(
                            Application.Current.TryFindResource("CategoryDeleted") as string ?? "Kategorija je obrisana."
                        ).ShowDialog();

                        LoadCategories();
                    }
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
