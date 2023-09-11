using FireSharp.Interfaces;
using FireSharp.Response;
using Google.Cloud.Firestore;
using ManagerApp.Model;
using ManagerApp.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Authentication.Identity.Core;

namespace ManagerApp.Repository
{
    class AccountRepository : FirebaseConfiguration, IAccountRepository
    {
        static Dictionary<string, Model.Type> typeDict = new Dictionary<string, Model.Type> {
            { "Standard", Model.Type.Standard },
            { "VIP", Model.Type.VIP },
            { "NotUsingApp", Model.Type.NotUsingApp }
        };

        static Dictionary<string, Model.Role> roleDict = new Dictionary<string, Model.Role> {
            { "Admin", Model.Role.Admin },
            { "User", Model.Role.User },
            { "Customer", Model.Role.Customer },
            { "Driver", Model.Role.Driver }
        };

        static Dictionary<string, Model.AccountStatus> statusDict = new Dictionary<string, Model.AccountStatus> {
            { "None", Model.AccountStatus.None },
            { "Restricted", Model.AccountStatus.Restricted }
        };

        static Dictionary<string, Model.Gender> genderDict = new Dictionary<string, Model.Gender> {
            { "Male", Model.Gender.Male },
            { "Female", Model.Gender.Female },
            { "Unspecified", Model.Gender.Unspecified }
        };


        public Task<bool> AccountSignedIn(AdminAccount account)
        {
            return Task.Run(() => { return true; });
        }

        public Task<bool> AccountSignedUp(AdminAccount account)
        {
            return Task.Run(() => { return true; });
        }

        public Task<bool> AddAdminAccount(AdminAccount newAccount)
        {
            return Task.Run(() => { return true; });
        }

        public async Task<bool> AddCustomer(Customer newCustomer)
        {
            DocumentReference docRef = FirestoreDb.Collection("Customer").Document();

            Dictionary<string, object> customerFormattedObject = new Dictionary<string, object>
            {
                { "avatar", newCustomer.Avatar },
                { "bookingList", new ArrayList() },
                { "email", newCustomer.Email },
                { "name", newCustomer.Name },
                { "dob", DateTime.SpecifyKind(newCustomer.DateOfBirth, DateTimeKind.Utc) },
                { "phoneNumber", newCustomer.PhoneNumber },
                { "saved_locations", new ArrayList() },
                { "type_customer", newCustomer.Type.ToString() },
                { "created_at", DateTime.SpecifyKind(newCustomer.CreatedAt, DateTimeKind.Utc)},
                { "updated_at", DateTime.SpecifyKind(newCustomer.UpdatedAt, DateTimeKind.Utc)},
                { "gender", newCustomer.Gender.ToString() },
                { "status", newCustomer.Status.ToString() },
                { "create_by_admin", newCustomer.CreateByAdmin },
                { "fcm_token", "" },
            };

            WriteResult result = await docRef.SetAsync(customerFormattedObject);
            if (result != null)
            {
                return true;
            }
            return false;
        }

        public Task<bool> AddDriver(Driver newDriver)
        {
            return Task.Run(() => { return true; });
        }

        public Task<bool> DeleteAdminAccount(string accountId)
        {
            return Task.Run(() => { return true; });
        }

        public async Task<bool> DeleteCustomer(string customerId)
        {
            DocumentReference docRef = FirestoreDb.Collection("Customer").Document(customerId);
            WriteResult result = await docRef.DeleteAsync();
            if (result != null) { return true;} return false;
        }

        public Task<bool> DeleteDriver(string driverId)
        {
            return Task.Run(() => { return true; });
        }

        public Task<bool> EditAdminAccount(AdminAccount currentAccount)
        {
            return Task.Run(() => { return true; });
        }

        public async Task<bool> EditCustomer(Customer currentCustomer)
        {
            DocumentReference docRef = FirestoreDb.Collection("Customer").Document(currentCustomer.Id);

            Dictionary<string, object> customerFormattedObject = new Dictionary<string, object>
            {
                { "avatar", currentCustomer.Avatar },
                //{ "bookingList", new ArrayList() },
                { "email", currentCustomer.Email },
                { "name", currentCustomer.Name },
                { "dob",  DateTime.SpecifyKind(currentCustomer.DateOfBirth, DateTimeKind.Utc) },
                { "phoneNumber", currentCustomer.PhoneNumber },
                //{ "
                //
                //d_locations", new ArrayList() },
                { "type_customer", currentCustomer.Type.ToString() },
                { "updated_at", DateTime.SpecifyKind(currentCustomer.UpdatedAt, DateTimeKind.Utc) },
                { "gender", currentCustomer.Gender.ToString() },
                { "status", currentCustomer.Status.ToString() },
                //{ "create_by_admin", currentCustomer.CreateByAdmin },
                //{ "fcm_token", "" },
            };

            WriteResult result = await docRef.UpdateAsync(customerFormattedObject);
            if (result != null)
            {
                return true;
            }
            return false;
        }

        public Task<bool> EditDriver(Customer currentDriver)
        {
            return Task.Run(() => { return true; });
        }

        public Task<Account> GetAccountById(string accountId)
        {
            return Task.Run(() => { return new Account(); });
        }

        public Task<ObservableCollection<Account>> GetAll()
        {
            return Task.Run(() => { return new ObservableCollection<Account>(); });
        }

        public async Task<List<Customer>> GetAllCustomer()
        {
            List<Customer> customers = new List<Customer>();

            Query allCustomersQuery = FirestoreDb.Collection("Customer");
            QuerySnapshot allCustomersQuerySnapshot = await allCustomersQuery.GetSnapshotAsync();
            foreach (DocumentSnapshot documentSnapshot in allCustomersQuerySnapshot.Documents)
            {
                Customer current = new Customer
                {
                    Id = documentSnapshot.Id,
                };
                Dictionary<string, object> customerFormattedObject = documentSnapshot.ToDictionary();
                foreach (KeyValuePair<string, object> pair in customerFormattedObject)
                {
                    if (pair.Key.Equals("avatar")) { current.Avatar = pair.Value?.ToString(); continue; };
                    if (pair.Key.Equals("created_by_admin")) { current.CreateByAdmin = pair.Value?.ToString(); continue; };
                    if (pair.Key.Equals("created_at")) { current.CreatedAt = pair.Value is Timestamp timestamp ? timestamp.ToDateTime().ToLocalTime() : current.CreatedAt; continue; };
                    if (pair.Key.Equals("dob")) { current.DateOfBirth = pair.Value is Timestamp timestamp ? timestamp.ToDateTime().ToLocalTime() : current.DateOfBirth; continue; };
                    if (pair.Key.Equals("email")) { current.Email = pair.Value?.ToString(); continue; };
                    if (pair.Key.Equals("fcm_token")) { continue; };
                    if (pair.Key.Equals("gender")) { current.Gender = genderDict[pair.Value?.ToString()]; continue; };
                    if (pair.Key.Equals("name")) { current.Name = pair.Value?.ToString(); continue; };
                    if (pair.Key.Equals("phoneNumber")) { current.PhoneNumber = pair.Value?.ToString(); continue; };
                    if (pair.Key.Equals("saved_locations")) {

                        var locationList = pair.Value as List<object>;
                        current.Location = new List<string>();
                        if (locationList != null) current.Location = locationList.OfType<string>().Select(item => item.ToString()).ToList();
                        continue; 
                    };
                    if (pair.Key.Equals("status")) { current.Status = statusDict[pair.Value?.ToString()]; continue; };
                    if (pair.Key.Equals("type_customer")) { current.Type = typeDict[pair.Value?.ToString()]; continue; };
                    if (pair.Key.Equals("updated_at")) { current.UpdatedAt = pair.Value is Timestamp timestamp ? timestamp.ToDateTime().ToLocalTime() : current.UpdatedAt; continue; };
                }
                customers.Add(current);
            }
            return customers;
        }

        public Task<ObservableCollection<Driver>> GetAllDriver()
        {
            return Task.Run(() => { return new ObservableCollection<Driver>(); });
        }

        public Task<bool> GetCustomerById(string customerId)
        {
            return Task.Run(() => { return true; });
        }

        public Task<int> GetCustomerListCount()
        {
            return Task.Run(() => { return 1; });
        }

        public Task<bool> GetDriverById(string driverId)
        {
            return Task.Run(() => { return true; });
        }
    }
}
