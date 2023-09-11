using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerApp.Model
{
    public enum Gender
    {
        Male,
        Female,
        Unspecified
    }

    public enum Type
    {
        Standard,
        VIP,
        NotUsingApp
    }

    public enum AccountStatus
    {
        None,
        Restricted
    }

    [FirestoreData]
    public class Customer: UserAccount
    {
        protected string _email;
        protected List<string> _saved_locations;
        protected DateTime _dateOfBirth;
        protected Gender _gender;
        protected Type _type;
        protected DateTime _createdAt;
        protected DateTime _updatedAt;
        protected string _createdByAdmin;
        protected AccountStatus _status;

        [FirestoreProperty]
        public string Email { get => _email; set => _email = value; }
        [FirestoreProperty]
        public List<string> Location { get => _saved_locations; set => _saved_locations = value; }
        [FirestoreProperty]
        public DateTime DateOfBirth { get => _dateOfBirth; set => _dateOfBirth = value; }
        [FirestoreProperty]
        public Gender Gender { get => _gender; set => _gender = value; }
        [FirestoreProperty]
        public Type Type { get => _type; set => _type = value; }
        [FirestoreProperty]
        public DateTime CreatedAt { get => _createdAt; set => _createdAt = value; }
        [FirestoreProperty]
        public DateTime UpdatedAt { get => _updatedAt; set => _updatedAt = value; }
        [FirestoreProperty]
        public string CreateByAdmin { get => _createdByAdmin; set => _createdByAdmin = value; }
        [FirestoreProperty]
        public AccountStatus Status { get => _status; set => _status = value;   }

        public Customer() { this.Type = Type.Standard; this.Role = Role.Customer; }
    }
}
