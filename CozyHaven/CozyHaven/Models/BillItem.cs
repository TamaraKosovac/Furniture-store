using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace CozyHaven.Models
{
    public class BillItem : INotifyPropertyChanged
    {
        public int Id { get; set; }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(TotalPrice));
                }
            }
        }

        public decimal UnitPrice { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        [NotMapped]
        public decimal TotalPrice => UnitPrice * Quantity;

        public int BillId { get; set; }
        public Bill Bill { get; set; } = null!;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
