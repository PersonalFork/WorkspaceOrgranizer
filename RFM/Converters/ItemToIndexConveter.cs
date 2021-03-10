using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace RFM.Converters
{
    public class ItemToIndexConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2)
            {
                return null;
            }
            if (values[0] is IEnumerable<object> items && values[1] is object item)
            {
                int index = items.ToList().IndexOf(item);
                if (index == -1)
                {
                    return null;
                }
                return (index + 1).ToString();
            }
            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
