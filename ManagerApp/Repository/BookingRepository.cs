using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.DependencyInjection;
using FireSharp;
using FireSharp.Interfaces;
using FireSharp.Response;
using ManagerApp.Model;
using ManagerApp.Services;
using Windows.Media.Protection.PlayReady;

namespace ManagerApp.Repository
{
    class BookingRepository : FirebaseConfiguration, IBookingRepository
    {
        public bool SetBookingDetail(IFirebaseClient client, BookingDetail booking)
        {
            try
            {
                SetResponse res = client.Set("Bookings/" + booking.Id + "/PhoneNumber", booking.PhoneNumber);
                res = client.Set("Bookings/" + booking.Id + "/CustomerRole", booking.CustomerRole.ToString());
                res = client.Set("Bookings/" + booking.Id + "/PickupLocationName", booking.PickupLocationName);
                res = client.Set("Bookings/" + booking.Id + "/DestinationName", booking.DestinationName);
                res = client.Set("Bookings/" + booking.Id + "/PickupTime", booking.PickupTime.ToString());
                res = client.Set("Bookings/" + booking.Id + "/PickupDate", booking.PickupDate.ToString());
                res = client.Set("Bookings/" + booking.Id + "/Price", booking.Price.ToString());
                res = client.Set("Bookings/" + booking.Id + "/Rating", booking.Rating.ToString());
                res = client.Set("Bookings/" + booking.Id + "/Transport", booking.Transport);
                res = client.Set("Bookings/" + booking.Id + "/Status", booking.Status.ToString());
            }
            catch 
            {
                return false;
            }
            
            return true;
        }

        public bool GetBookingDetail(IFirebaseClient client, ref BookingDetail booking)
        {
            try
            {
                var res = client.Get("Bookings/" + booking.Id + "/PhoneNumber");
                booking.PhoneNumber = res.ResultAs<string>();

                res = client.Get("Bookings/" + booking.Id + "/CustomerRole");
                booking.CustomerRole = int.Parse(res.ResultAs<string>());

                res = client.Get("Bookings/" + booking.Id + "/PickupLocationName");
                booking.PickupLocationName = res.ResultAs<string>();

                res = client.Get("Bookings/" + booking.Id + "/DestinationName");
                booking.DestinationName = res.ResultAs<string>();

                res = client.Get("Bookings/" + booking.Id + "/PickupTime");
                booking.PickupTime = TimeSpan.Parse(res.ResultAs<string>());

                res = client.Get("Bookings/" + booking.Id + "/PickupDate");
                booking.PickupDate = DateOnly.Parse(res.ResultAs<string>());

                res = client.Get("Bookings/" + booking.Id + "/Price");
                booking.Price = int.Parse(res.ResultAs<string>());

                res = client.Get("Bookings/" + booking.Id + "/Rating");
                booking.Rating = int.Parse(res.ResultAs<string>());

                res = client.Get("Bookings/" + booking.Id + "/Transport");
                booking.Transport = res.ResultAs<string>();

                res = client.Get("Bookings/" + booking.Id + "/Status");
                booking.Status = int.Parse(res.ResultAs<string>());
            }
            catch
            {
                return false;
            }
            return true;
        }

        public async Task<bool> Add(BookingDetail booking)
        {
            IFirebaseClient client;

            try
            {
                client = await TryConnect();
            }
            catch
            {
                await App.MainRoot.ShowDialog("Can not connect to Firebase", "There was a problem with your internet!");
                return false;
            }

            bool result = SetBookingDetail(client,booking);

            if (result)
            {
                await App.MainRoot.ShowDialog("Success", "Add Booking Successfully!");
            }
            else
            {
                await App.MainRoot.ShowDialog("Error", "Add Booking Failed...");
            }
            return result;
        }

        public async Task<bool> Delete(int id)
        {
            return true;
        }

        public async Task<bool> Edit(BookingDetail booking)
        {
            return true;
        }

        public async Task<ObservableCollection<BookingDetail>> GetAll()
        {
            ObservableCollection<BookingDetail> bookings = new ObservableCollection<BookingDetail>();
            IFirebaseClient client;

            try
            {
                client = await TryConnect();
            }
            catch
            {
                await App.MainRoot.ShowDialog("Can not connect to Firebase", "There was a problem with your internet!");
                return bookings;
            }

            var res = client.Get("Bookings/count");
            if (res.Body == null) return bookings;
            int count = int.Parse(res.ResultAs<string>());
            if (count == 0) return bookings;

            List<int> idList = new List<int>();

            for (int i = 0; i < count; i++)
            {
                res = client.Get("Bookings/idList/" + i);
                if (res.Body == null) return bookings;
                int temp = int.Parse(res.ResultAs<string>());
                idList.Add(temp);
            }

            foreach (int id in idList)
            {
                BookingDetail temp = new BookingDetail();
                temp.Id = id;
                bool result = GetBookingDetail(client, ref temp);

                if (!result) { 
                    await App.MainRoot.ShowDialog("Error", "Unable to fetch data... Please check database entries");
                    break;
                }
                bookings.Add(temp);
            }

            return bookings;
        }

        public Task<BookingDetail> GetById(int id)
        {
            return Task.Run(() => { return new BookingDetail(); });
        }
    }
}
