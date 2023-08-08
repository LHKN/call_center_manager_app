using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerApp.Model
{
    class BookingDetail
    {
        private int _id;
        private int _customerPhone;
        private int _customerRole;
        //private int _customerId;

        private DateOnly? _orderDate;
        private int _price;
        private int _rating;
        private int _status;

        //transport means
        private int _driverId;
        private int _transportType;

        //locations
        private string _pickupLocation;
        private string _destination;

        //datetime
        private DateOnly? _departureDate;
        // departureTime hh:mm:ss?


        public int Id { get => _id; set => _id = value; }
        public int CustomerPhone { get => _customerPhone; set => _customerPhone = value; }
        public int CustomerRole { get => _customerRole; set => _customerRole = value; }
        
        
        public DateOnly? DepartureDate { get => _departureDate; set => _departureDate = value; }
    }
}
