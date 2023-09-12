using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerApp.Model
{
    [FirestoreData]
    partial class Driver: UserAccount
    {
        public enum DriverStatus
        {
            Offline,
            Available,
            Busy,
            Restricted
        }
        protected EWallet _eWallet;
        protected float _rating;
        protected string _licensePlate;
        protected Gender _gender;
        protected DateTime _dateOfBirth;
        private DriverStatus _status;
        private AccountStatus _accountStatus;
        private DateTime _createdAt;
        private DateTime _updatedAt;
        private string _email;

        [FirestoreProperty]
        public EWallet EWallet { get => _eWallet; set => _eWallet = value; }
        [FirestoreProperty]
        public float Rating { get => _rating; set => _rating = value; }
        [FirestoreProperty]
        public string LicensePlate { get => _licensePlate; set => _licensePlate = value; }
        [FirestoreProperty]
        public Gender Gender { get => _gender; set => _gender = value; }
        [FirestoreProperty]
        public DateTime DateOfBirth { get => _dateOfBirth; set => _dateOfBirth = value; }
        [FirestoreProperty]
        public DriverStatus Status { get => _status; set => _status = value; }
        [FirestoreProperty]
        public AccountStatus AccountStatus { get => _accountStatus; set => _accountStatus = value; }
        [FirestoreProperty]
        public DateTime CreatedAt { get => _createdAt; set => _createdAt = value; }
        [FirestoreProperty]
        public DateTime UpdatedAt { get => _updatedAt; set => _updatedAt = value; }
        [FirestoreProperty]
        public string Email { get => _email; set => _email = value; }

        public Driver() { this.Role = Role.Driver; }
    }
}
