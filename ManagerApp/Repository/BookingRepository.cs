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
            SetResponse res = client.Set("Bookings/" + booking.Id + "/PhoneNumber", booking.PhoneNumber);
            if (res.Body == null) return false;

            res = client.Set("Bookings/" + booking.Id + "/CustomerRole", booking.CustomerRole.ToString());
            if (res.Body == null) return false;

            res = client.Set("Bookings/" + booking.Id + "/PickupLocationName", booking.PickupLocationName);
            if (res.Body == null) return false;

            res = client.Set("Bookings/" + booking.Id + "/DestinationName", booking.DestinationName);
            if (res.Body == null) return false;

            res = client.Set("Bookings/" + booking.Id + "/PickupTime", booking.PickupTime.ToString());
            if (res.Body == null) return false;

            res = client.Set("Bookings/" + booking.Id + "/PickupDate", booking.PickupDate.ToString());
            if (res.Body == null) return false;

            res = client.Set("Bookings/" + booking.Id + "/Price", booking.Price.ToString());
            if (res.Body == null) return false;

            res = client.Set("Bookings/" + booking.Id + "/Rating", booking.Rating.ToString());
            if (res.Body == null) return false;

            res = client.Set("Bookings/" + booking.Id + "/Transport", booking.Transport);
            if (res.Body == null) return false;

            res = client.Set("Bookings/" + booking.Id + "/Status", booking.Status.ToString());
            if (res.Body == null) return false;

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
            throw new NotImplementedException();
        }

        public async Task<bool> Edit(BookingDetail booking)
        {
            throw new NotImplementedException();
        }

        public Task<ObservableCollection<BookingDetail>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<BookingDetail> GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
