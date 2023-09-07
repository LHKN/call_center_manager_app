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
        Normal,
        VIP
    }

    public enum AccountStatus
    {
        None,
        Restricted
    }
    public class Customer: Account
    {
        //protected string _id;
        //protected string _name;
        protected string _email;
        //protected string _phoneNumber;
        protected string _location;
        protected DateOnly _dateOfBirth;
        protected Gender _gender;
        protected Type _type;
        protected DateTime _createdAt;
        protected DateTime _updatedAt;
        protected string _createdByAdmin;
        protected AccountStatus _status;

        public string Email { get => _email; set => _email = value; }
        public string Location { get => _location; set => _location = value; }
        public DateOnly DateOfBirth { get => _dateOfBirth; set => _dateOfBirth = value; }
        public Gender Gender { get => _gender; set => _gender = value; }
        public Type Type { get => _type; set => _type = value; }
        public DateTime CreatedAt { get => _createdAt; set => _createdAt = value; }
        public DateTime UpdatedAt { get => _updatedAt; set => _updatedAt = value; }
        public string CreateByAdmin { get => _createdByAdmin; set => _createdByAdmin = value; }
        public AccountStatus Status { get => _status; set => _status = value;   }

        public Customer() { this.Type = Type.Normal; }
    }
}
