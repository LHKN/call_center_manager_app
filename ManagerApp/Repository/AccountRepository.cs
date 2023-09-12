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
using System.Runtime.InteropServices;
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

        static Dictionary<string, Model.Driver.DriverStatus> driverStatusDict = new Dictionary<string, Model.Driver.DriverStatus> {
            { "offline", Model.Driver.DriverStatus.Offline },
            { "available", Model.Driver.DriverStatus.Available },
            { "busy", Model.Driver.DriverStatus.Busy },
            { "restricted", Model.Driver.DriverStatus.Restricted },
            
        };

        static Dictionary<string, Model.Gender> genderDict = new Dictionary<string, Model.Gender> {
            { "Male", Model.Gender.Male },
            { "Female", Model.Gender.Female },
            { "Unspecified", Model.Gender.Unspecified }
        };
        static Dictionary<string, Model.Gender> genderDictIIConvert = new Dictionary<string, Model.Gender> {
            { "True", Model.Gender.Female },
            { "False", Model.Gender.Male },
        };
        static Dictionary<Model.Gender, bool> genderDictIIConvertBack = new Dictionary<Model.Gender, bool> {
            { Model.Gender.Female, true  },
            { Model.Gender.Male, false  },
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
                { "_status", newCustomer.Status.ToString() },
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

        public async Task<bool> AddDriver(Driver newDriver)
        {
            DocumentReference driverDocRef = FirestoreDb.Collection("driver").Document();
            DocumentReference driver_partitionDocRef = FirestoreDb.Collection("driver_partition").Document($"{driverDocRef.Id}");

            Dictionary<string, object> driverFormattedObject = new Dictionary<string, object>
            {
                { "avatar", newDriver.Avatar },
                { "email", newDriver.Email },
                { "name", newDriver.Name },
                { "e_wallet", new ArrayList() },
                { "dob", DateTime.SpecifyKind(newDriver.DateOfBirth, DateTimeKind.Utc) },
                { "phoneNumber", newDriver.PhoneNumber },
                { "created_at", DateTime.SpecifyKind(newDriver.CreatedAt, DateTimeKind.Utc)},
                { "updated_at", DateTime.SpecifyKind(newDriver.UpdatedAt, DateTimeKind.Utc)},
                { "gender", genderDictIIConvertBack[newDriver.Gender] },
                //{ "rating", ((int)Math.Round(newDriver.Rating)).ToString() },
                { "rating", 0.ToString() },
            };

            Dictionary<string, object> driverPartitionFormattedObject = new Dictionary<string, object>
            {
                { "driver_status", newDriver.Status.ToString().ToLower() },
                { "area_status", "" },
                { "fcm_token", "" },
                { "partition_key", "" },
            };

            WriteResult resultI = await driverDocRef.SetAsync(driverFormattedObject);
            WriteResult resultII = await driver_partitionDocRef.SetAsync(driverPartitionFormattedObject);
            if (resultI != null && resultII != null)
            {
                return true;
            }
            return false;
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

        public async Task<bool> DeleteDriver(string driverId)
        {
            DocumentReference docRef = FirestoreDb.Collection("driver").Document(driverId);
            DocumentReference docPartitionRef = FirestoreDb.Collection("driver_partition").Document(driverId);
            WriteResult resultI = await docRef.DeleteAsync();
            WriteResult resultII = await docPartitionRef.DeleteAsync();
            if (resultI != null && resultII != null) { return true; }
            return false;
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
                { "_status", currentCustomer.Status.ToString() },
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

        public async Task<bool> EditDriver(Driver currentDriver)
        {
            DocumentReference driverDocRef = FirestoreDb.Collection("driver").Document(currentDriver.Id);
            DocumentReference driver_partitionDocRef = FirestoreDb.Collection("driver_partition").Document(currentDriver.Id);

            Dictionary<string, object> driverFormattedObject = new Dictionary<string, object>
            {
                { "avatar", currentDriver.Avatar },
                { "email", currentDriver.Email },
                { "name", currentDriver.Name },
                { "e_wallet", new ArrayList() },
                { "dob", DateTime.SpecifyKind(currentDriver.DateOfBirth, DateTimeKind.Utc) },
                { "phoneNumber", currentDriver.PhoneNumber },
                { "created_at", DateTime.SpecifyKind(currentDriver.CreatedAt, DateTimeKind.Utc)},
                { "updated_at", DateTime.SpecifyKind(currentDriver.UpdatedAt, DateTimeKind.Utc)},
                { "gender", genderDictIIConvertBack[currentDriver.Gender] },
                //{ "rating", ((int)Math.Round(newDriver.Rating)).ToString() },
            };

            Dictionary<string, object> driverPartitionFormattedObject = new Dictionary<string, object>
            {
                { "driver_status", currentDriver.Status.ToString().ToLower() },
                { "area_status", "" },
                { "fcm_token", "" },
                { "partition_key", "" },
            };

            WriteResult resultI = await driverDocRef.UpdateAsync(driverFormattedObject);
            WriteResult resultII = await driver_partitionDocRef.UpdateAsync(driverPartitionFormattedObject);
            if (resultI != null && resultII != null)
            {
                return true;
            }
            return false;
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

        public async Task<List<Driver>> GetAllDriver()
        {
            List<Driver> drivers = new List<Driver>();

            Query driverQuery = FirestoreDb.Collection("driver");
            QuerySnapshot allDriverQuerySnapshot = await driverQuery.GetSnapshotAsync();
            foreach (DocumentSnapshot documentSnapshot in allDriverQuerySnapshot.Documents)
            {
                Driver current = new Driver
                {
                    Id = documentSnapshot.Id,
                };
                Dictionary<string, object> customerFormattedObject = documentSnapshot.ToDictionary();
                DocumentReference partitionDocRef = FirestoreDb.Collection("driver_partition").Document($"{current.Id}");
                DocumentSnapshot snapshot = await partitionDocRef.GetSnapshotAsync();


                foreach (KeyValuePair<string, object> pair in customerFormattedObject)
                {
                    if (pair.Key.Equals("avatar")) { current.Avatar = pair.Value?.ToString(); continue; };
                    if (pair.Key.Equals("created_at")) { current.CreatedAt = pair.Value is Timestamp timestamp ? timestamp.ToDateTime().ToLocalTime() : current.CreatedAt; continue; };
                    if (pair.Key.Equals("dob")) { current.DateOfBirth = pair.Value is Timestamp timestamp ? timestamp.ToDateTime().ToLocalTime() : current.DateOfBirth; continue; };
                    if (pair.Key.Equals("email")) { current.Email = pair.Value?.ToString(); continue; };
                    //if (pair.Key.Equals("fcm_token")) { continue; };
                    if (pair.Key.Equals("gender")) { current.Gender = genderDictIIConvert[pair.Value?.ToString()]; continue; };
                    if (pair.Key.Equals("name")) { current.Name = pair.Value?.ToString(); continue; };
                    if (pair.Key.Equals("phoneNumber")) { current.PhoneNumber = pair.Value?.ToString(); continue; };
                    if (pair.Key.Equals("updated_at")) { current.UpdatedAt = pair.Value is Timestamp timestamp ? timestamp.ToDateTime().ToLocalTime() : current.UpdatedAt; continue; };

                    if (snapshot.Exists)
                    {
                        Dictionary<string, object> partitionDict = snapshot.ToDictionary();
                        foreach (KeyValuePair<string, object> _pair in partitionDict)
                        {
                            if (_pair.Key.Equals("area_status")) { continue; };
                            if (_pair.Key.Equals("driver_status")) { current.Status = driverStatusDict[_pair.Value?.ToString()]; continue; };
                            if (_pair.Key.Equals("fcm_token")) { continue; };
                            if (_pair.Key.Equals("partition_key")) { continue; };
                        }
                    }
                }
                drivers.Add(current);
            }
            return drivers;
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
