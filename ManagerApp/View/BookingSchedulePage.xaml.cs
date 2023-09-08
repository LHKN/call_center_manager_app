// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI;
using ManagerApp.Model;
using System.Collections.ObjectModel;
using Windows.UI;
using Microsoft.UI.Xaml;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ManagerApp.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BookingSchedulePage : Page
    {
        private Dictionary<DateOnly?, ObservableCollection<BookingDetail>> Bookings;

        public BookingSchedulePage()
        {
            this.InitializeComponent();
            datePicker.MinYear = DateTimeOffset.Now;
        }

        private void CalendarView_CalendarViewDayItemChanging(CalendarView sender,
                                   CalendarViewDayItemChangingEventArgs args)
        {
            var allBookings = dataListView.ItemsSource as ObservableCollection<BookingDetail>;
            DateOnly selectedDate = DateOnly.FromDateTime(args.Item.Date.Date);

            // sort bookings into Bookings dict
            Bookings = new Dictionary<DateOnly?, ObservableCollection<BookingDetail>>();

            foreach (BookingDetail booking in allBookings)
            {
                if (Bookings.ContainsKey(booking.PickupDate) == false)
                {
                    Bookings.Add(booking.PickupDate, new ObservableCollection<BookingDetail>());
                }
            }

            foreach (BookingDetail booking in allBookings) {
                Bookings[booking.PickupDate].Add(booking);
            }

            if (Bookings.ContainsKey(selectedDate) == false)
            {
                Bookings.Add(selectedDate, new ObservableCollection<BookingDetail>());
            }

            // Render basic day items.
            if (args.Phase == 0)
            {
                // Register callback for next phase.
                args.RegisterUpdateCallback(CalendarView_CalendarViewDayItemChanging);
            }
            // Set blackout dates.
            else if (args.Phase == 1)
            {
                // Blackout dates in the past, Sundays, and dates that are fully booked.
                if (args.Item.Date < DateTimeOffset.Now.Date
                        //|| Bookings[selectedDate].Count == <<booking limit>> 
                        )
                {
                    args.Item.IsBlackout = true;
                }
                // Register callback for next phase.
                args.RegisterUpdateCallback(CalendarView_CalendarViewDayItemChanging);
            }
            // Set density bars.
            else if (args.Phase == 2)
            {
                // Avoid unnecessary processing.
                // You don't need to set bars on past dates or Sundays.
                if (args.Item.Date >= DateTimeOffset.Now.Date)
                {
                    // Get bookings for the date being rendered.
                    var currentBookings = Bookings[selectedDate];

                    List<Color> densityColors = new List<Color>();
                    // Set a density bar color for each of the days bookings.
                    // It's assumed that there can't be more than 10 bookings in a day. Otherwise,
                    // further processing is needed to fit within the max of 10 density bars.
                    foreach (BookingDetail booking in currentBookings)
                    {
                        if (booking.Status == 0)
                        {
                            densityColors.Add(Colors.Green);
                        }
                        else
                        {
                            densityColors.Add(Colors.Blue);
                        }
                    }
                    args.Item.SetDensityColors(densityColors);
                }
            }
        }

        //private void CalendarView_SelectedDatesChanged(CalendarView sender, CalendarViewSelectedDatesChangedEventArgs args)
        //{
        //    string date = sender.SelectedDates.FirstOrDefault().Date.ToString();
        //    date = date.Split(" ").FirstOrDefault();
        //    selectedDate.Text = date;
        //}
    }
}
