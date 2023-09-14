using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerApp.Services
{
    class BookingStatusConverter : IValueConverter
    {
        static Dictionary<int, string> types = new Dictionary<int, string> {
            { 0, "Inactive"},
            { 1, "Active" }
        };

        public object Convert(object value, System.Type targetType, object parameter, string language)
        {
            int status = (int)value;
            return types[status];
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, string language)
        {
            string typeString = (string)value;
            return types.FirstOrDefault(x => x.Value.Equals(typeString)).Key;
        }
    }
}
