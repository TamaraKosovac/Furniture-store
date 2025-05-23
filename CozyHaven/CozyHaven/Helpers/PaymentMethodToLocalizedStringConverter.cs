using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace CozyHaven.Helpers
{
    public class PaymentMethodToLocalizedStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return string.Empty;

            var key = value.ToString(); 
            return Application.Current.TryFindResource(key) as string ?? key!;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
