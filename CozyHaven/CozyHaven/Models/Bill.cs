using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CozyHaven.Models
{
    public enum PaymentMethod
    {
        Cash,
        Card
    }

    public class Bill : INotifyPropertyChanged
    {
        public int Id { get; set; }

        private DateTime _dateTime = DateTime.Now;
        public DateTime DateTime
        {
            get => _dateTime;
            set
            {
                if (_dateTime != value)
                {
                    _dateTime = value;
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

        public decimal TotalAmount { get; set; }
        public List<BillItem> Items { get; set; } = new();
        public int CashRegisterId { get; set; }
        public CashRegister CashRegister { get; set; } = null!;
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
