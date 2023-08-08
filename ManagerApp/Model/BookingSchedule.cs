using System;

namespace ManagerApp.Model
{
    public sealed class BookingSchedule
    {
        private int _id;
        private int _customerPhone;
        private int _customerStatus;
        private DateOnly? _departureDate;

        public int Id { get => _id; set => _id = value; }
        public int CustomerPhone { get => _customerPhone; set => _customerPhone = value; }
        public int CustomerStatus { get => _customerStatus; set => _customerStatus = value; }
        public DateOnly? DepartureDate { get => _departureDate; set => _departureDate = value; }
    }

}