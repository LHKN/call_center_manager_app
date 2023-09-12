using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using FireSharp.Interfaces;
using ManagerApp.Model;
using ManagerApp.Services;

namespace ManagerApp.Repository
{
    class StatisticsRepository : FirebaseConfiguration, IStatisticsRepository
    {
        private List<string> _monthLabels = new List<string>
            {
                "Week 1","Week 2","Week 3","Week 4",
            };

        private List<string> _yearLabels = new List<string>
            {
                "JAN",
                "FEB",
                "MAR",
                "APR",
                "MAY",
                "JUN",
                "JUL",
                "AUG",
                "SEP",
                "OCT",
                "NOV",
                "DEC"
            };

        public async Task<int> GetCustomerCount()
        {
            return 0;
        }

        public async Task<int> GetDriverCount()
        {
            return 0;
        }

        public async Task<double> GetTripPercentage()
        {
            return 80;
        }

        public async Task<List<Tuple<string, int>>> GetIncomeByMonth(int month, int year)
        {
            return null;
        }

        public async Task<List<Tuple<string, int>>> GetIncomeByYear(int year)
        {
            return null;
        }
    }
}
