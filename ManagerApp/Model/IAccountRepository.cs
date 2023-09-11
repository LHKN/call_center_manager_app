using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

        Task<bool> AccountSignedUp(AdminAccount account);
        Task<bool> AccountSignedIn(AdminAccount account);

        Task<int> GetCustomerListCount();

        Task<bool> AddCustomer(Customer newCustomer);
        Task<bool> EditCustomer(Customer currentCustomer);
        Task<bool> DeleteCustomer(string customerId);
        Task<bool> GetCustomerById(string customerId);
        
        Task<bool> AddDriver(Driver newDriver);
        Task<bool> EditDriver(Customer currentDriver);
        Task<bool> DeleteDriver(string driverId);
        Task<bool> GetDriverById(string driverId);

        Task<ObservableCollection<Account>> GetAll();
        Task<List<Customer>> GetAllCustomer();
        Task<ObservableCollection<Driver>> GetAllDriver();
    }
}
