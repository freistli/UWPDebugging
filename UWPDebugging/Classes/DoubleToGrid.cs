using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace UWPDebugging.Classes
{
    public class DoubleToGrid : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var n = (double)value;
            return new GridLength(n);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
