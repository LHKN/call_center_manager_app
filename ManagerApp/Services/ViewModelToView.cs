using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using ManagerApp.View;
using ManagerApp.ViewModel;
using System;
using System.Collections.Generic;
using Windows.UI.ApplicationSettings;

namespace ManagerApp.Services
{
    internal class ViewModelToView : IValueConverter
    {
        private static readonly Dictionary<Type, Type> pairs = new Dictionary<Type, Type>()
        {
            //{typeof(LoginViewModel), typeof(LoginPage)},
            {typeof(HomeViewModel), typeof(HomePage)},
            {typeof(StatisticsViewModel), typeof(StatisticsPage)},
            {typeof(BookingScheduleViewModel), typeof(BookingSchedulePage)},

            //{typeof(AddBookingViewModel), typeof(AddBookingPage)},
            //{typeof(ViewBookingViewModel), typeof(ViewBookingPage)},
     
            //{typeof(EditBookingViewModel), typeof(EditBookingPage)},
     
            {typeof(ManageCustomerViewModel), typeof(ManageCustomerPage)},
            //{typeof(AddCustomerViewModel), typeof(AddCustomerPage)},

            {typeof(ManageDriverViewModel), typeof(ManageDriverPage)},
            //{typeof(AddDriverViewModel), typeof(AddDriverPage)},

            {typeof(LogsViewModel), typeof(LogsPage)},

            //add more page...

        };

        object IValueConverter.Convert(object value, Type targetType, object parameter, string language)
        {
            pairs.TryGetValue(value.GetType(), out var page);
            Page x = (Page)Activator.CreateInstance(page);
            x.DataContext = value;
            return x;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
