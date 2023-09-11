using CommunityToolkit.Mvvm.ComponentModel;
using Google.Cloud.Firestore;
using System.ComponentModel;

namespace ManagerApp.Model
{
    public enum Role
    {
        Admin = 1,
        User = 2,
        Customer = 3,
        Driver = 4,
    }

    [FirestoreData]
    public class Account : ObservableObject
    {
        protected string _id;
        protected string _name;
        protected string _phoneNumber;
        protected string _avatar;
        protected string _username;
        protected Role _role;

        [FirestoreProperty]
        public string Id { get => _id; set => _id = value; }
        [FirestoreProperty]
        public string Name { get => _name; set => _name = value; }
        [FirestoreProperty]
        public string PhoneNumber { get => _phoneNumber; set => _phoneNumber = value; }
        [FirestoreProperty]
        public string Username { get => _username; set => _username = value; }
        [FirestoreProperty]
        public Role Role { get => _role; set => _role = value; }
        [FirestoreProperty]
        public string Avatar { get => _avatar; set => _avatar = value; }

    }
}
