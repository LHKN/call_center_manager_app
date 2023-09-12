using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ManagerApp.Model
{
    interface IStatisticsRepository
    {
        Task<int> GetCustomerCount();
        Task<int> GetDriverCount();
        Task<double> GetTripPercentage();
        Task<List<Tuple<string, int>>> GetIncomeByMonth(int month, int year);
        Task<List<Tuple<string, int>>> GetIncomeByYear(int year);
    }
}
