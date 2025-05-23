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
    public class AddProductViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Category> Categories { get; set; } = new();

        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        private decimal? _supplierPrice;
        public decimal? SupplierPrice
        {
            get => _supplierPrice;
            set { _supplierPrice = value; OnPropertyChanged(); }
        }

        private decimal? _price;
        public decimal? Price
        {
            get => _price;
            set { _price = value; OnPropertyChanged(); }
        }

        private int? _availableQuantity;
        public int? AvailableQuantity
        {
            get => _availableQuantity;
            set { _availableQuantity = value; OnPropertyChanged(); }
        }

        private string _composition = string.Empty;
        public string Composition
        {
            get => _composition;
            set { _composition = value; OnPropertyChanged(); }
        }

        private Category? _selectedCategory;
        public Category? SelectedCategory
        {
            get => _selectedCategory;
            set { _selectedCategory = value; OnPropertyChanged(); }
        }

        private string _description = string.Empty;
        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(); }
        }

        private string? _imagePath;
        public string? ImagePath
        {
            get => _imagePath;
            set { _imagePath = value; OnPropertyChanged(); }
        }

        public ICommand SaveProductCommand => new RelayCommand(SaveProduct);

        private Product? _editingProduct;

        public AddProductViewModel()
        {
            LoadCategories();
        }

        public AddProductViewModel(Product product)
        {
            _editingProduct = product;
            LoadCategories();

            Name = product.Name;
            SupplierPrice = product.SupplierPrice;
            Price = product.Price;
            AvailableQuantity = product.AvailableQuantity;
            Composition = product.Composition;
            Description = product.Description;
            ImagePath = product.ImagePath;
            SelectedCategory = Categories.FirstOrDefault(c => c.Id == product.CategoryId);
        }

        private void LoadCategories()
        {
            using var context = new AppDbContext();
            var allCategories = context.Categories.ToList();

            Categories.Clear();
            foreach (var category in allCategories)
                Categories.Add(category);
        }

        private void SaveProduct(object? obj)
        {
            if (string.IsNullOrWhiteSpace(Name)
                || SupplierPrice == null
                || Price == null
                || AvailableQuantity == null
                || SelectedCategory == null)
            {
                new MessageBoxView(GetStringResource("EmptyFields")).ShowDialog();
                return;
            }

            using var context = new AppDbContext();

            if (_editingProduct == null)
            {
                var newProduct = new Product
                {
                    Name = Name,
                    SupplierPrice = SupplierPrice ?? 0,
                    Price = Price ?? 0,
                    AvailableQuantity = AvailableQuantity ?? 0,
                    Composition = Composition,
                    CategoryId = SelectedCategory.Id,
                    Description = Description,
                    ImagePath = ImagePath
                };

                context.Products.Add(newProduct);
                context.SaveChanges();
                new MessageBoxView(GetStringResource("ProductAdded")).ShowDialog();
            }
            else
            {
                var existing = context.Products.Find(_editingProduct.Id);
                if (existing != null)
                {
                    existing.Name = Name;
                    existing.SupplierPrice = SupplierPrice ?? 0;
                    existing.Price = Price ?? 0;
                    existing.AvailableQuantity = AvailableQuantity ?? 0;
                    existing.Composition = Composition;
                    existing.CategoryId = SelectedCategory.Id;
                    existing.Description = Description;
                    existing.ImagePath = ImagePath;

                    context.SaveChanges();
                }
            }

            if (obj is Window window)
            {
                window.DialogResult = true;
                window.Close();
            }
        }


        private string GetStringResource(string key)
        {
            return Application.Current.TryFindResource(key)?.ToString() ?? key;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
