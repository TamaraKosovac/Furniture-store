using CozyHaven.Data;
using CozyHaven.Models;
using CozyHaven.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using CozyHaven.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace CozyHaven.ViewModels
{
    public class PaymentMethodDisplay
    {
        public PaymentMethod Value { get; set; }
        public string DisplayName { get; set; }


        public PaymentMethodDisplay(PaymentMethod value)
        {
            Value = value;
            DisplayName = GetLocalizedDisplayName(value);
        }

        private string GetLocalizedDisplayName(PaymentMethod method)
        {
            return Application.Current.TryFindResource(method.ToString()) as string ?? method.ToString();
        }

        public override string ToString() => DisplayName;
    }
    public class ProductsViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Category> FilterOptions { get; set; } = new();
        public ObservableCollection<Product> Products { get; set; } = new();
        public ObservableCollection<BillItem> SelectedProducts { get; set; } = new();
        public ObservableCollection<CashRegister> CashRegisters { get; set; } = new();
        public ObservableCollection<PaymentMethodDisplay> PaymentMethods { get; set; } =
            new ObservableCollection<PaymentMethodDisplay>(
                Enum.GetValues(typeof(PaymentMethod))
                    .Cast<PaymentMethod>()
                    .Select(pm => new PaymentMethodDisplay(pm))
            );

        public decimal TotalAmount => SelectedProducts.Sum(item => item.Quantity * item.UnitPrice);

        private Bill? _currentBill;

        private CashRegister? _selectedCashRegister;
        public CashRegister? SelectedCashRegister
        {
            get => _selectedCashRegister;
            set
            {
                if (_selectedCashRegister != value)
                {
                    _selectedCashRegister = value;
                    OnPropertyChanged();
                }
            }
        }


        private PaymentMethodDisplay _selectedPaymentMethodDisplay = null!;
        public PaymentMethodDisplay SelectedPaymentMethodDisplay
        {
            get => _selectedPaymentMethodDisplay;
            set
            {
                if (_selectedPaymentMethodDisplay != value)
                {
                    _selectedPaymentMethodDisplay = value;
                    PaymentMethod = value.Value;
                    OnPropertyChanged();
                }
            }
        }

        private PaymentMethod _paymentMethod;
        public PaymentMethod PaymentMethod
        {
            get => _paymentMethod;
            set
            {
                if (_paymentMethod != value)
                {
                    _paymentMethod = value;
                    OnPropertyChanged();
                }
            }
        }

        private Category _selectedFilter = null!;
        public Category SelectedFilter
        {
            get => _selectedFilter;
            set
            {
                if (_selectedFilter != value)
                {
                    _selectedFilter = value;
                    OnPropertyChanged();
                    ApplyFilter();
                }
            }
        }

        public ICommand UpdateProductCommand { get; }
        public ICommand DeleteProductCommand { get; }
        public ICommand ViewProductCommand { get; }
        public ICommand AddToBillCommand { get; }
        public ICommand RemoveFromBillCommand { get; }
        public ICommand IncreaseQuantityCommand { get; }
        public ICommand IssueBillCommand { get; }

        private readonly User? _loggedInUser;
        public ICommand DecreaseQuantityCommand { get; }

        public ProductsViewModel()
        {
            UpdateProductCommand = new RelayCommand(UpdateProduct);
            DeleteProductCommand = new RelayCommand(DeleteProduct);
            ViewProductCommand = new RelayCommand(ViewProduct);
            AddToBillCommand = new RelayCommand<Product>(AddToBill);
            RemoveFromBillCommand = new RelayCommand<BillItem>(RemoveFromBill);
            IncreaseQuantityCommand = new RelayCommand<BillItem>(IncreaseQuantity);
            IssueBillCommand = new RelayCommand(_ => IssueBill());
            DecreaseQuantityCommand = new RelayCommand<BillItem>(DecreaseQuantity);

            LoadCategories();
            LoadProducts();
            LoadCashRegisters();

            SelectedPaymentMethodDisplay = PaymentMethods.FirstOrDefault(p => p.Value == PaymentMethod.Cash)!;
        }

        public ProductsViewModel(User loggedInUser) : this()
        {
            _loggedInUser = loggedInUser;
        }

        private void LoadCategories()
        {
            using var context = new AppDbContext();
            var categories = context.Categories
                .Where(c => !c.IsDeleted)
                .ToList();

            FilterOptions.Clear();
            FilterOptions.Add(new Category { Id = 0, Name = "Sve kategorije" });

            foreach (var cat in categories)
                FilterOptions.Add(cat);

            SelectedFilter = FilterOptions.First();
        }

        private void DecreaseQuantity(BillItem billItem)
        {
            if (billItem == null) return;

            using var context = new AppDbContext();

            var itemInDb = context.BillItems.FirstOrDefault(b => b.Id == billItem.Id);
            var productInDb = context.Products.FirstOrDefault(p => p.Id == billItem.ProductId);
            var localProduct = Products.FirstOrDefault(p => p.Id == billItem.ProductId);

            if (itemInDb != null && productInDb != null)
            {
                productInDb.AvailableQuantity += 1;
                if (localProduct != null)
                    localProduct.AvailableQuantity += 1;

                if (itemInDb.Quantity > 1)
                {
                    itemInDb.Quantity -= 1;
                    billItem.Quantity -= 1; 
                }
                else
                {
                    context.BillItems.Remove(itemInDb);
                    SelectedProducts.Remove(billItem);
                }

                context.SaveChanges();

                OnPropertyChanged(nameof(AvailableProducts));
                OnPropertyChanged(nameof(TotalAmount));
            }
        }


        private void LoadCashRegisters()
        {
            using var context = new AppDbContext();
            var registers = context.CashRegisters.ToList();

            CashRegisters.Clear();
            foreach (var register in registers)
                CashRegisters.Add(register);

            SelectedCashRegister = CashRegisters.FirstOrDefault();
            OnPropertyChanged(nameof(SelectedCashRegister));
        }

        private void ApplyFilter()
        {
            LoadProducts();
        }

        public void LoadProducts()
        {
            using var context = new AppDbContext();

            var products = context.Products
                .Include(p => p.Category)
                .Where(p =>
                    !p.IsDeleted &&
                    !p.Category.IsDeleted &&
                    (SelectedFilter == null || SelectedFilter.Id == 0 || p.CategoryId == SelectedFilter.Id))
                .Select(p => new Product
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = Math.Round(p.Price, 2),
                    SupplierPrice = Math.Round(p.SupplierPrice, 2),
                    AvailableQuantity = p.AvailableQuantity,
                    Composition = p.Composition,
                    Description = p.Description,
                    ImagePath = p.ImagePath,
                    CategoryId = p.CategoryId,
                    Category = p.Category
                })
                .ToList();

            Products.Clear();
            foreach (var product in products)
                Products.Add(product);

            OnPropertyChanged(nameof(AvailableProducts));
        }

        private void UpdateProduct(object parameter)
        {
            if (parameter is Product product)
            {
                var window = new AddProductView(product);
                window.Owner = Application.Current.MainWindow;
                bool? result = window.ShowDialog();

                LoadProducts();

                if (result == true)
                {
                    string updatedMessage = string.Format(
                        TryFindLocalizedString("ProductUpdatedSuccess"), product.Name);

                    var message = new MessageBoxView(updatedMessage);
                    message.Owner = Application.Current.MainWindow;
                    message.ShowDialog();
                }
            }
        }

        private void DeleteProduct(object parameter)
        {
            if (parameter is Product product)
            {
                string confirmationMessage = string.Format(
                    TryFindLocalizedString("DeleteProductConfirmation"), product.Name);

                var dialog = new ConfirmationDialogView(confirmationMessage);
                dialog.Owner = Application.Current.MainWindow;

                if (dialog.ShowDialog() == true)
                {
                    using var context = new AppDbContext();

                    var entity = context.Products.Find(product.Id);
                    if (entity != null)
                    {
                        entity.IsDeleted = true;
                        context.SaveChanges();
                        LoadProducts();

                        string successMessage = string.Format(
                            TryFindLocalizedString("ProductDeletedSuccess"), product.Name);

                        var message = new MessageBoxView(successMessage);
                        message.Owner = Application.Current.MainWindow;
                        message.ShowDialog();
                    }
                }
            }
        }


        private void ViewProduct(object parameter)
        {
            if (parameter is Product product)
            {
                var window = new ProductDetailsView(product);
                window.Owner = Application.Current.MainWindow;
                window.ShowDialog();
            }
        }

        private void AddToBill(Product product)
        {
            if (product == null) return;
            if (_loggedInUser == null)
            {
                return;
            }

            using var context = new AppDbContext();

            if (_currentBill == null)
            {
                if (SelectedCashRegister == null) return;
                PaymentMethod = SelectedPaymentMethodDisplay.Value;
                _currentBill = new Bill
                {
                    DateTime = DateTime.Now,
                    PaymentMethod = PaymentMethod,
                    TotalAmount = 0,
                    UserId = _loggedInUser.Id,
                    CashRegisterId = SelectedCashRegister.Id
                };

                context.Bills.Add(_currentBill);
                context.SaveChanges();
            }

            var productInDb = context.Products.FirstOrDefault(p => p.Id == product.Id);
            if (productInDb == null || productInDb.AvailableQuantity <= 0)
                return;

            bool alreadyAdded = context.BillItems.Any(b => b.ProductId == product.Id && b.BillId == _currentBill.Id);
            if (alreadyAdded) return;

            productInDb.AvailableQuantity -= 1;
            context.SaveChanges();

            var billItem = new BillItem
            {
                ProductId = product.Id,
                Quantity = 1,
                UnitPrice = product.Price,
                BillId = _currentBill.Id
            };

            context.BillItems.Add(billItem);
            context.SaveChanges();

            LoadSelectedProducts();
            product.AvailableQuantity -= 1;
            OnPropertyChanged(nameof(AvailableProducts));
            OnPropertyChanged(nameof(TotalAmount));
        }

        private void RemoveFromBill(BillItem billItem)
        {
            if (billItem == null) return;

            SelectedProducts.Remove(billItem);

            using var context = new AppDbContext();

            var productInDb = context.Products.FirstOrDefault(p => p.Id == billItem.ProductId);
            if (productInDb != null)
            {
                productInDb.AvailableQuantity += 1;
            }

            var billItemInDb = context.BillItems.FirstOrDefault(b => b.Id == billItem.Id);
            if (billItemInDb != null)
            {
                context.BillItems.Remove(billItemInDb);
            }

            context.SaveChanges();

            var localProduct = Products.FirstOrDefault(p => p.Id == billItem.ProductId);
            if (localProduct != null)
                localProduct.AvailableQuantity += 1;

            OnPropertyChanged(nameof(AvailableProducts));
            OnPropertyChanged(nameof(TotalAmount));
        }

        private void IssueBill()
        {
            if (_currentBill == null)
            {
                var msg = new MessageBoxView(TryFindLocalizedString("BillNotStarted"));
                msg.Owner = Application.Current.MainWindow;
                msg.ShowDialog();
                return;
            }

            using var context = new AppDbContext();

            var items = context.BillItems.Where(b => b.BillId == _currentBill.Id).ToList();
            if (items.Count == 0)
            {
                var msg = new MessageBoxView(TryFindLocalizedString("BillEmpty"));
                msg.Owner = Application.Current.MainWindow;
                msg.ShowDialog();
                return;
            }

            var billInDb = context.Bills.FirstOrDefault(b => b.Id == _currentBill.Id);
            if (billInDb == null)
            {
                var msg = new MessageBoxView(TryFindLocalizedString("BillNotFound"));
                msg.Owner = Application.Current.MainWindow;
                msg.ShowDialog();
                return;
            }
            billInDb.PaymentMethod = SelectedPaymentMethodDisplay?.Value ?? PaymentMethod.Cash;

            billInDb.TotalAmount = items.Sum(i => i.Quantity * i.UnitPrice);
            context.SaveChanges();


            _currentBill = null;
            SelectedProducts.Clear();
            LoadProducts();
            OnPropertyChanged(nameof(SelectedProducts));
            OnPropertyChanged(nameof(TotalAmount));

            var billMessageBox = new BillMessageBox();
            billMessageBox.ShowDialog();
        }

        private void IncreaseQuantity(BillItem billItem)
        {
            if (billItem == null) return;

            using var context = new AppDbContext();

            var productInDb = context.Products.FirstOrDefault(p => p.Id == billItem.ProductId);
            if (productInDb == null || productInDb.AvailableQuantity <= 0)
                return;

            productInDb.AvailableQuantity -= 1;

            var billItemInDb = context.BillItems.FirstOrDefault(b => b.Id == billItem.Id);
            if (billItemInDb != null)
            {
                billItemInDb.Quantity += 1;
            }

            context.SaveChanges();

            billItem.Quantity += 1;

            var localProduct = Products.FirstOrDefault(p => p.Id == billItem.ProductId);
            if (localProduct != null)
                localProduct.AvailableQuantity -= 1;

            OnPropertyChanged(nameof(AvailableProducts));
            OnPropertyChanged(nameof(TotalAmount));
        }

        private void LoadSelectedProducts()
        {
            if (_currentBill == null) return;

            using var context = new AppDbContext();

            var items = context.BillItems
                .Include(b => b.Product)
                .Where(b => b.BillId == _currentBill.Id)
                .ToList();

            SelectedProducts.Clear();
            foreach (var item in items)
                SelectedProducts.Add(item);

            OnPropertyChanged(nameof(SelectedProducts));
        }

        public void RevertUnpaidItems()
        {
            if (_currentBill == null)
                return;

            using var context = new AppDbContext();

            var billItems = context.BillItems
                .Include(b => b.Product)
                .Where(b => b.BillId == _currentBill.Id)
                .ToList();

            foreach (var item in billItems)
            {
                var productInDb = context.Products.FirstOrDefault(p => p.Id == item.ProductId);
                if (productInDb != null)
                {
                    productInDb.AvailableQuantity += item.Quantity;
                }

                context.BillItems.Remove(item);
            }

            var bill = context.Bills.FirstOrDefault(b => b.Id == _currentBill.Id);
            if (bill != null)
            {
                context.Bills.Remove(bill);
            }

            context.SaveChanges();
            _currentBill = null;

            SelectedProducts.Clear();
            LoadProducts();
            OnPropertyChanged(nameof(SelectedProducts));
            OnPropertyChanged(nameof(TotalAmount));
        }


        private string TryFindLocalizedString(string key)
        {
            return Application.Current.TryFindResource(key) as string ?? key;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public IEnumerable<Product> AvailableProducts => Products.Where(p => !p.IsDeleted && p.AvailableQuantity > 0);

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}