using System.ComponentModel;


namespace ManagerApp.Model
{
    public class EWallet : INotifyPropertyChanged
    {
        protected int _id;
        protected string _userId;
        protected string _phoneNumber;
        protected int _balance;

        public int Id { get => _id; set => _id = value; }
        public string UserId { get => _userId; set => _userId = value; }
        public string PhoneNumber { get => _phoneNumber; set => _phoneNumber = value; }
        public int Balance { get => _balance; set => _balance = value; }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
