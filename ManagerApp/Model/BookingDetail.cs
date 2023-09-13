using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerApp.Model
{
    public class BookingDetail : ObservableObject
    {
        private int _id;
        private string _phoneNumber;
        private string _customerRole;
        private string _customerName;

        //private DateOnly? _orderDate;
        private int _price;
        private string _duration;
        private string _distance;
        //private int _rating;
        private int _status;

        //transport means
        private int _driverId;
        private string _transport;

        //locations
        private string _pickupLocationName;
        private string _destinationName;
        // coordinates
        private double _pickupLocationLatitude;
        private double _pickupLocationLongitude;
        private double _destinationLatitude;
        private double _destinationLongitude;

        //datetime
        private TimeSpan? _pickupTime;
        private DateOnly? _pickupDate;

        public int Id { get => _id; set => _id = value; }
        public string PhoneNumber { get => _phoneNumber; set => _phoneNumber = value; }
        public string CustomerRole { get => _customerRole; set => _customerRole = value; }
        public string CustomerName { get => _customerName; set => _customerName = value; }
        public string PickupLocationName { get => _pickupLocationName; set => _pickupLocationName = value; }
        public string DestinationName { get => _destinationName; set => _destinationName = value; }
        public TimeSpan? PickupTime { get => _pickupTime; set => _pickupTime = value; }
        public DateOnly? PickupDate { get => _pickupDate; set => _pickupDate = value; }
        public int Price { get => _price; set => _price = value; }
        //public int Rating { get => _rating; set => _rating = value; }
        public string Transport { get => _transport; set => _transport = value; }
        public int Status { get => _status; set => _status = value; }
        public double PickupLocationLatitude { get => _pickupLocationLatitude; set => _pickupLocationLatitude = value; }
        public double PickupLocationLongitude { get => _pickupLocationLongitude; set => _pickupLocationLongitude = value; }
        public double DestinationLatitude { get => _destinationLatitude; set => _destinationLatitude = value; }
        public double DestinationLongitude { get => _destinationLongitude; set => _destinationLongitude = value; }
        public string Duration { get => _duration; set => _duration = value; }
        public string Distance { get => _distance; set => _distance = value; }

        public bool CheckNullDetail()
        {
            return (PhoneNumber != null && PickupLocationName != null && DestinationName != null);
        }

        public bool CheckValidDate(DateTime date, TimeSpan time)
        {
            DateTime bookingDate = ((DateOnly)this.PickupDate).ToDateTime(TimeOnly.MinValue);
            TimeSpan bookingTime = (TimeSpan)this.PickupTime;

            if (bookingDate.Date.CompareTo(date.Date) < 0)
            {
                return false;
            }
            else if (bookingDate.Date.CompareTo(date.Date) == 0)
            {
                if (bookingTime.CompareTo(time) < 0)
                {
                    return false;
                }
                else if (bookingTime.CompareTo(time) == 0)
                {
                    this.Status = 1;
                    return true;
                }
            }
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
