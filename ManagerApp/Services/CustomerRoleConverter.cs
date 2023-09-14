using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerApp.Services
{
    class CustomerRoleConverter : IValueConverter
    {
        static Dictionary<string, string> types = new Dictionary<string, string> {
            { "0", "Standard"},
            { "1", "VIP" },
            { "2", "Not Using App" }
        };

        public object Convert(object value, System.Type targetType, object parameter, string language)
        {
            string customerType = (string)value;
            return types[customerType];
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, string language)
        {
            string typeString = (string)value;
            return types.FirstOrDefault(x => x.Value.Equals(typeString)).Key;
        }
    }
}
