using CommunityToolkit.Mvvm.Input;
using ManagerApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ManagerApp.ViewModel
{
    partial class ManageCustomerViewModel : ViewModelBase
    {
        // fields
        private List<Customer> _customerList;
        private List<Customer> _displayCustomerList;
        private ObservableCollection<Customer> _displayCustomerCollection;

        // constructor
        public ManageCustomerViewModel()
        {
            _customerList = new List<Customer> {
                new VIPCustomer {
                    Id = "CUS000001",
                    Name = "Trần Văn Tèo",
                    PhoneNumber = "+84834635633",
                    Location = "89 Hoàng Hoa Thám, Phường 6, Bình Thạnh, Thành phố Hồ Chí Minh, Việt Nam",

                },
                new Customer
                {
                    Id = "CUS000002",
                    Name = "Thái Văn Thiên",
                    PhoneNumber = "+84192355906",
                    Location = "71/1/12 Nguyễn Văn Thương, Phường 25, Bình Thạnh, Thành phố Hồ Chí Minh, Việt Nam",

                },
                new VIPCustomer
                {
                    Id = "CUS000003",
                    Name = "Lê Hoàng Khanh Nguyên",
                    PhoneNumber = "+84331197205",
                    Location = "106 Phạm Viết Chánh, Phường Nguyễn Cư Trinh, Quận 1, Thành phố Hồ Chí Minh, Việt Nam",

                }

            };
            _displayCustomerList = new List<Customer>(_customerList);
            _displayCustomerCollection = new ObservableCollection<Customer>();
            _displayCustomerList.ForEach(customer => _displayCustomerCollection.Add(customer));
            AddCommand = new RelayCommand(ExecuteAddCommand);
        }

        // execute commands
        public async void ExecuteAddCommand()
        {
            ParentPageNavigation.ViewModel = new AddCustomerViewModel();
        }

        // getters, setters
        public List<Customer> CustomerList { get => _customerList; set => _customerList = value; }
        public List<Customer> DisplayCustomerList { get => _displayCustomerList; set => _displayCustomerList = value; }
        public ObservableCollection<Customer> DisplayCustomerCollection { get => _displayCustomerCollection; set => _displayCustomerCollection = value; }


        // commands
        public ICommand AddCommand { get; }
    }
}
