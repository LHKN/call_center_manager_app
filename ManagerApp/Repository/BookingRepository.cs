using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManagerApp.Model;
using ManagerApp.Services;

namespace ManagerApp.Repository
{
    class BookingRepository : FirebaseConfiguration, IBookingRepository
    {
        public async Task<bool> Add(BookingDetail booking)
        {
            try
            {
                TryConnect();
                ClientSetBooking("Bookings/" + booking.Id.ToString(), booking);
                await App.MainRoot.ShowDialog("Success", "Add Booking Successfully!");
                return true;
            }
            catch(Exception ex)
            {
                await App.MainRoot.ShowDialog("Error", ex.Message);
                return false;
            }
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
