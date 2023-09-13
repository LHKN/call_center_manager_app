namespace ManagerApp.Model
{
    class BookingDetailTemplate
    {
        private string _id;
        private string _phoneNumber;
        private string _customerRole;
        private string _customerName;

        private string _orderDate;
        private string _price;
        private string _duration;
        private string _distance;
        private string _rating;
        private string _status;

        //transport means
        private string _driverId;
        private string _transport;

        //locations
        private string _pickupLocationName;
        private string _destinationName;
        // coordinates
        private string _pickupLocationLatitude;
        private string _pickupLocationLongitude;
        private string _destinationLatitude;
        private string _destinationLongitude;

        //datetime
        private string _pickupTime;
        private string _pickupDate;

        public string Id { get => _id; set => _id = value; }
        public string PhoneNumber { get => _phoneNumber; set => _phoneNumber = value; }
        public string CustomerRole { get => _customerRole; set => _customerRole = value; }
        public string CustomerName { get => _customerName; set => _customerName = value; }
        public string PickupLocationName { get => _pickupLocationName; set => _pickupLocationName = value; }
        public string DestinationName { get => _destinationName; set => _destinationName = value; }
        public string PickupTime { get => _pickupTime; set => _pickupTime = value; }
        public string PickupDate { get => _pickupDate; set => _pickupDate = value; }
        public string Price { get => _price; set => _price = value; }
        public string Rating { get => _rating; set => _rating = value; }
        public string Transport { get => _transport; set => _transport = value; }
        public string Status { get => _status; set => _status = value; }
        public string PickupLocationLatitude { get => _pickupLocationLatitude; set => _pickupLocationLatitude = value; }
        public string PickupLocationLongitude { get => _pickupLocationLongitude; set => _pickupLocationLongitude = value; }
        public string DestinationLatitude { get => _destinationLatitude; set => _destinationLatitude = value; }
        public string DestinationLongitude { get => _destinationLongitude; set => _destinationLongitude = value; }
        public string Duration { get => _duration; set => _duration = value; }
        public string Distance { get => _distance; set => _distance = value; }
    }
}
