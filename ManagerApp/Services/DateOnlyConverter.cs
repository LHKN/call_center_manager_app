using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerApp.Services
{
    class DateOnlyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
            {
                return value;
            }
            return new DateTimeOffset(((DateOnly)value).ToDateTime(TimeOnly.MinValue).ToUniversalTime());
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return DateOnly.FromDateTime(((DateTimeOffset)value).Date);
        }
    }
}
