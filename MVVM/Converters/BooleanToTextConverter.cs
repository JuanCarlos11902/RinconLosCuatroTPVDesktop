using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace rinconLosCuatroTPVDesktop.MVVM.Converters
{
    internal class BooleanToTextConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            string cadena;
            if (value is bool and true)
            {
                cadena = "Actual: Disponible para el Cliente";
            }
            else
            {
                cadena = "Actual: No disponible para el Cliente";
            }

            return cadena;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
