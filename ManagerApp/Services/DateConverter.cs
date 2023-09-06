using System;
using Microsoft.UI.Xaml.Data;

namespace ManagerApp.Services
{
    internal class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null) { 
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
