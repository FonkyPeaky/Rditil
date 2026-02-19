using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Rditil.Converters
{
    /// <summary>
    /// Retourne Visible si la valeur numérique est > 0, sinon Collapsed.
    /// Gère int, long, double, decimal, string numérique.
    /// </summary>
    public class ZeroToVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return Visibility.Collapsed;

            try
            {
                double number = value switch
                {
                    int i => i,
                    long l => l,
                    float f => f,
                    double d => d,
                    decimal m => (double)m,
                    string s when double.TryParse(s, NumberStyles.Any, culture, out var n) => n,
                    _ => System.Convert.ToDouble(value, culture)
                };

                return number > 0 ? Visibility.Visible : Visibility.Collapsed;
            }
            catch
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            Binding.DoNothing;
    }
}
