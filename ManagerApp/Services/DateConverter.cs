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
            return new DateTimeOffset(((DateTime)value).ToUniversalTime());
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return DateTime.SpecifyKind(((DateTimeOffset)value).Date, DateTimeKind.Utc);
        }
    
    }
}
