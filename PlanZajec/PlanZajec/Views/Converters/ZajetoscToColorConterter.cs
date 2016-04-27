using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace PlanZajec.Views.Converters
{
    class ZajetoscToColorConterter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            System.Diagnostics.Debug.Write(value.GetType());
            System.Diagnostics.Debug.Write("Target type = "+targetType.ToString());
            bool? zajety = value as bool?;
            Color colResult = (zajety.HasValue && zajety.Value) ? Colors.OrangeRed : Colors.LightGreen;
            SolidColorBrush solidResult = new SolidColorBrush(colResult);
            return colResult;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
