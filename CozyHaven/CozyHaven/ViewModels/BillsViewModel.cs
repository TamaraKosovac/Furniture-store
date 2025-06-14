using CozyHaven.Data;
using CozyHaven.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CozyHaven.ViewModels
{
    public class BillDisplay
    {
        public int Number { get; set; }
        public Bill Bill { get; set; } = null!;
    }

    public class BillsViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<BillDisplay> Bills { get; set; } = new();
        public ObservableCollection<DateTime> FilterOptions { get; set; } = new();

        private DateTime? _selectedFilter;
        public DateTime? SelectedFilter
        {
            get => _selectedFilter;
            set
            {
                _selectedFilter = value;
                OnPropertyChanged();
                FilterBills();
            }
        }

        public BillsViewModel()
        {
            LoadBills();
        }

        private void LoadBills()
        {
            using var context = new AppDbContext();
            var billsFromDb = context.Bills
                .Include(b => b.Items)
                    .ThenInclude(i => i.Product)
                .Include(b => b.CashRegister)
                .OrderByDescending(b => b.DateTime)
                .ToList();

            Bills.Clear();
            int counter = 1;
            foreach (var bill in billsFromDb)
            {
                Bills.Add(new BillDisplay
                {
                    Number = counter++,
                    Bill = bill
                });
            }

            FilterOptions.Clear();
            foreach (var date in billsFromDb.Select(b => b.DateTime.Date).Distinct())
                FilterOptions.Add(date);
        }

        private void FilterBills()
        {
            using var context = new AppDbContext();
            var query = context.Bills
                .Include(b => b.Items)
                    .ThenInclude(i => i.Product)
                .Include(b => b.CashRegister)
                .AsQueryable();

            if (SelectedFilter != null)
            {
                var date = SelectedFilter.Value.Date;
                query = query.Where(b => b.DateTime.Date == date);
            }

            var filtered = query.ToList();
            Bills.Clear();
            int counter = 1;
            foreach (var bill in filtered)
            {
                Bills.Add(new BillDisplay
                {
                    Number = counter++,
                    Bill = bill
                });
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}