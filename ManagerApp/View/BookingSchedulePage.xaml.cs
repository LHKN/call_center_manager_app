// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI;
using Microsoft.UI.Xaml.Documents;
using ManagerApp.Model;
using System.Collections.ObjectModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ManagerApp.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BookingSchedulePage : Page
    {
        private Dictionary<DateOnly, ObservableCollection<BookingDetail>> Bookings;

        public BookingSchedulePage()
        {
            this.InitializeComponent();
        }

        private void CalendarView_CalendarViewDayItemChanging(CalendarView sender,
                                   CalendarViewDayItemChangingEventArgs args)
        {
            var allBookings = dataListView.ItemsSource as ObservableCollection<BookingDetail>;

            // sort bookings into Bookings dict

            //// Render basic day items.
            //if (args.Phase == 0)
            //{
            //    // Register callback for next phase.
            //    args.RegisterUpdateCallback(CalendarView_CalendarViewDayItemChanging);
            //}
            //// Set blackout dates.
            //else if (args.Phase == 1)
            //{
            //    // Blackout dates in the past, Sundays, and dates that are fully booked.
            //    if (args.Item.Date < DateTimeOffset.Now ||
            //        args.Item.Date.DayOfWeek == DayOfWeek.Sunday ||
            //        Bookings.HasOpenings(args.Item.Date) == false)
            //    {
            //        args.Item.IsBlackout = true;
            //    }
            //    // Register callback for next phase.
            //    args.RegisterUpdateCallback(CalendarView_CalendarViewDayItemChanging);
            //}
            //// Set density bars.
            //else if (args.Phase == 2)
            //{
            //    // Avoid unnecessary processing.
            //    // You don't need to set bars on past dates or Sundays.
            //    if (args.Item.Date > DateTimeOffset.Now &&
            //        args.Item.Date.DayOfWeek != DayOfWeek.Sunday)
            //    {
            //        // Get bookings for the date being rendered.
            //        var currentBookings = Bookings.GetBookings(args.Item.Date);

            //        List<Color> densityColors = new List<Color>();
            //        // Set a density bar color for each of the days bookings.
            //        // It's assumed that there can't be more than 10 bookings in a day. Otherwise,
            //        // further processing is needed to fit within the max of 10 density bars.
            //        foreach (booking in currentBookings)
            //        {
            //            if (booking.IsConfirmed == true)
            //            {
            //                densityColors.Add(Colors.Green);
            //            }
            //            else
            //            {
            //                densityColors.Add(Colors.Blue);
            //            }
            //        }
            //        args.Item.SetDensityColors(densityColors);
            //    }
            //}
        }

        private void CalendarView_SelectedDatesChanged(CalendarView sender, CalendarViewSelectedDatesChangedEventArgs args)
        {
            string date = sender.SelectedDates.FirstOrDefault().Date.ToString();
            date = date.Split(" ").FirstOrDefault();
            selectedDate.Text = date;
        }
    }
}
