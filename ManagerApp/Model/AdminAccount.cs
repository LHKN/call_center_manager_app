using Google.Cloud.Firestore;
using System.ComponentModel;

namespace ManagerApp.Model
{
    [FirestoreData]
    public class AdminAccount : Account
    {
        public AdminAccount() { this.Role = Role.Admin; }
    }
}