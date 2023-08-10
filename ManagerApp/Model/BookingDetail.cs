using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerApp.Model
{
    class BookingDetail : INotifyPropertyChanged
    {
        private int _id;
        private string _phoneNumber;
        private int _customerRole;
        //private int _customerId;

        private DateOnly? _orderDate;
        private int _price;
        private int _rating;
        private int _status;

        //transport means
        private int _driverId;
        private int _transportType;
        private string _transport;

        //locations
        private string _pickupLocationName;
        private string _destinationName;
        // coordinates?

        //datetime
        private TimeSpan? _pickupTime;
        private DateTime? _pickupDate;

        public int Id { get => _id; set => _id = value; }
        public string PhoneNumber { get => _phoneNumber; set => _phoneNumber = value; }
        public int CustomerRole { get => _customerRole; set => _customerRole = value; }
        public string PickupLocationName { get => _pickupLocationName; set => _pickupLocationName = value; }
        public string DestinationName { get => _destinationName; set => _destinationName = value; }
        public TimeSpan? PickupTime { get => _pickupTime; set => _pickupTime = value; }
        public DateTime? PickupDate { get => _pickupDate; set => _pickupDate = value; }
        public int Price { get => _price; set => _price = value; }
        public int Rating { get => _rating; set => _rating = value; }
        public string Transport { get => _transport; set => _transport = value; }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
