using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ManagerApp.Model;

namespace ManagerApp.Repository
{
    class StatisticsRepository : FirebaseConfiguration, IStatisticsRepository
    {
        public StatisticsRepository(ObservableCollection<BookingDetail> initial)
        {
            bookingList = initial;
            Task.Run(() =>
            {
                while (true)
                {
                    Task.Delay(TimeSpan.FromHours(1));

                    var task = _bookingRepository.GetAll();
                    bookingList = task.Result;
                }
            });
        }

        private const int _initialValue = 0;

        private ObservableCollection<BookingDetail> bookingList;

        private IAccountRepository _accountRepository = new AccountRepository();
        private IBookingRepository _bookingRepository = new BookingRepository();

        public async Task<int> GetCustomerCount()
        {
            var customerList = await _accountRepository.GetAllCustomer();
            return customerList.Count;
        }

        public async Task<int> GetDriverCount()
        {
            var driverList = await _accountRepository.GetAllDriver();
            return driverList.Count;
        }

        public async Task<double> GetTripPercentage()
        {
            return 100;
        }

        public async Task<List<Tuple<string, int>>> GetIncomeByMonth(int month, int year)
        {
            var weeksTotalIncome = new List<Tuple<string, int>>
                {
                    new Tuple<string, int>("Week 1", _initialValue),
                    new Tuple<string, int>("Week 2", _initialValue),
                    new Tuple<string, int>("Week 3", _initialValue),
                    new Tuple<string, int>("Week 4", _initialValue)
                };

            foreach (var booking in bookingList)
            {
                var temp = ((DateOnly)booking.PickupDate).ToDateTime(TimeOnly.MinValue);
                if (temp.Year == year && temp.Month == month)
                {
                    int idx = 3;

                    if (temp.Day < 8)
                    {
                        idx = 0;
                    }
                    else if (temp.Day < 15)
                    {
                        idx = 1;
                    }
                    else if (temp.Day < 22)
                    {
                        idx = 2;
                    }

                    var newPrice = weeksTotalIncome[idx].Item2 + booking.Price;
                    Tuple<string, int> newTuple = new Tuple<string, int>(weeksTotalIncome[idx].Item1, newPrice);
                    weeksTotalIncome[idx] = newTuple;
                }
            }

            return weeksTotalIncome;
        }

        public async Task<List<Tuple<string, int>>> GetIncomeByYear(int year)
        {
            var yearsTotalIncome = new List<Tuple<string, int>>
                {
                    new Tuple<string, int>("JAN", _initialValue),
                    new Tuple<string, int>("FEB", _initialValue),
                    new Tuple<string, int>("MAR", _initialValue),
                    new Tuple<string, int>("APR", _initialValue),
                    new Tuple<string, int>("MAY", _initialValue),
                    new Tuple<string, int>("JUN", _initialValue),
                    new Tuple<string, int>("JUL", _initialValue),
                    new Tuple<string, int>("AUG", _initialValue),
                    new Tuple<string, int>("SEP", _initialValue),
                    new Tuple<string, int>("OCT", _initialValue),
                    new Tuple<string, int>("NOV", _initialValue),
                    new Tuple<string, int>("DEC", _initialValue)
                };

            foreach (var booking in bookingList)
            {
                var temp = ((DateOnly)booking.PickupDate).ToDateTime(TimeOnly.MinValue);
                if (temp.Year == year)
                {
                    var idx = temp.Month - 1;
                    var newPrice = yearsTotalIncome[idx].Item2 + booking.Price;
                    Tuple<string, int> newTuple = new Tuple<string, int>(yearsTotalIncome[idx].Item1, newPrice);
                    yearsTotalIncome[idx] = newTuple;
                }
            }

            return yearsTotalIncome;
        }
    }
}
