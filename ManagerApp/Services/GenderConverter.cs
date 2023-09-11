using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerApp.Services
{
    public class GenderConverter : IValueConverter
    {

        static Dictionary<Model.Gender, string> genders = new Dictionary<Model.Gender, string> {
            { Model.Gender.Male, "Male"},
            { Model.Gender.Female, "Female" },
            { Model.Gender.Unspecified, "Unspecified" },

        };

        public object Convert(object value, System.Type targetType, object parameter, string language)
        {
            Model.Gender customerGender = (Model.Gender)value;
            return genders[customerGender];
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, string language)
        {
            string genderString = (string)value;
            return genders.FirstOrDefault(x => x.Value.Equals(genderString)).Key;
        }
    }
}
