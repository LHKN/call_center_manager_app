using Microsoft.UI.Xaml.Data;
using System;
using System.ComponentModel;
using System.Globalization;

namespace ManagerApp.Services
{
    public class DoubleTriggerConverter : IValueConverter
    {
        public double Trigger { get; set; }
        public object TriggerValue { get; set; }
        public object DefaultValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var converter = TypeDescriptor.GetConverter(targetType);

            return (double)value == Trigger ? converter.ConvertFrom(TriggerValue) : converter.ConvertFrom(DefaultValue);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value, targetType, parameter, "");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ConvertBack(value, targetType, parameter, "");
        }
    }
}
