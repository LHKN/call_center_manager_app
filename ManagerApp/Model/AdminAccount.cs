using Google.Cloud.Firestore;
using System;
using System.ComponentModel;

namespace ManagerApp.Model
{
    [FirestoreData]
    public class AdminAccount : Account
    {
        private DateTime _dateOfBirth;
        private string _email;
        private Gender _gender;

        public AdminAccount() { this.Role = Role.Admin; }

        [FirestoreProperty]
        public DateTime DateOfBirth { get => _dateOfBirth; set => _dateOfBirth = value; }
        [FirestoreProperty]
        public string Email { get => _email; set => _email = value; }
        [FirestoreProperty]
        public Gender Gender { get => _gender; set => _gender = value; }
    }
}