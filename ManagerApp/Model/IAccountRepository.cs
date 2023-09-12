using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ManagerApp.Model
{
    interface IAccountRepository
    {
        Task<bool> AddAdminAccount(AdminAccount newAccount);
        Task<bool> EditAdminAccount(AdminAccount currentAccount);
        Task<bool> DeleteAdminAccount(string accountId);
        Task<Account> GetAccountById(string accountId);

        Task<bool> AccountSignedUp(NetworkCredential credentical);
        Task<bool> AccountSignedIn(NetworkCredential credentical);
        Task<bool> AccountSignedOut(NetworkCredential credentical);

        Task<int> GetCustomerListCount();
        Task<int> GetDriverListCount();

        Task<bool> AddCustomer(Customer newCustomer);
        Task<bool> EditCustomer(Customer currentCustomer);
        Task<bool> DeleteCustomer(string customerId);
        Task<bool> GetCustomerById(string customerId);
        
        Task<bool> AddDriver(Driver newDriver);
        Task<bool> EditDriver(Driver currentDriver);
        Task<bool> DeleteDriver(string driverId);
        Task<bool> GetDriverById(string driverId);

        Task<List<AdminAccount>> GetAllAdminAccount();
        Task<List<Customer>> GetAllCustomer();
        Task<List<Driver>> GetAllDriver();
    }
}
