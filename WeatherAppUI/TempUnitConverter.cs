using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WeatherAppUI
{
    class TempUnitConverter : IMultiValueConverter
    {
        public object Convert(
            object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string tempCelsius;
            string tempFahrenheit;
            bool isFahrenheit;
            if (values[0] != null)
                tempCelsius = values[0].ToString();
            else return "-";
            if (values[1] != null)
                tempFahrenheit = values[1].ToString();
            else return "-";
            if (values[2] != null)
                isFahrenheit = (bool)values[2];
            else return "-";

            return isFahrenheit ? tempFahrenheit : tempCelsius;
        }

        public object[] ConvertBack(
            object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
