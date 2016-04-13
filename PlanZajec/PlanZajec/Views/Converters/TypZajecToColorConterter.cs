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
    class TypZajecToColorConterter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string typZajec = value as string;
            Color colResult;
            switch (typZajec)
            {
                case "Wykład"               : colResult = Colors.Green;     break;
                case "Zajęcia laboratoryjne": colResult = Colors.Blue;      break;
                case "Projekt"              : colResult = Colors.Indigo;    break;
                case "Seminarium"           : colResult = Colors.LightBlue; break;
                case "Praktyka"             : colResult = Colors.Yellow;    break;
                default                     : colResult = Colors.Black;     break;
            }
            SolidColorBrush solidResult = new SolidColorBrush(colResult);
            return solidResult;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
