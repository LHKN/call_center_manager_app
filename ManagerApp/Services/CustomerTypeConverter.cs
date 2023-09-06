using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManagerApp.Model;
using Microsoft.UI.Xaml.Data;

namespace ManagerApp.Services
{
    public class CustomerTypeConverter: IValueConverter
    {
        static Dictionary<Model.Type, string> types = new Dictionary<Model.Type, string> {
            { Model.Type.Normal, "Normal"},
            { Model.Type.VIP, "VIP" }
        };

        public object Convert(object value, System.Type targetType, object parameter, string language)
        {
            Model.Type customerType = (Model.Type)value;
            return types[customerType];
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, string language)
        {
            string typeString = (string)value;
            return types.FirstOrDefault(x => x.Value.Equals(typeString)).Key;
        }
    }
}
