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
        
        private EWallet eWallet;
        private float rating;

        [FirestoreProperty]
        public EWallet EWallet { get => eWallet; set => eWallet = value; }
        [FirestoreProperty]
        public float Rating { get => rating; set => rating = value; }

        public Driver() { this.Role = Role.Driver; }
    }
}
