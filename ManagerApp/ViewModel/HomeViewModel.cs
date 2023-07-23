using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ManagerApp.ViewModel
{
    public class HomeViewModel : ViewModelBase
    {
        //private Account _account;

        //public HomeViewModel(Account account)
        //{
        //    Account = account;
        //    ChildPageNavigation = new PageNavigation(new StatisticsViewModel());
        //}
        public HomeViewModel()
        {
            //Account = null;
            ChildPageNavigation = new PageNavigation(new StatisticsViewModel());
        }

        private ICommand _itemInvokedCommand;
        public ICommand ItemInvokedCommand => _itemInvokedCommand ?? (_itemInvokedCommand = new RelayCommand<NavigationViewItemInvokedEventArgs>(OnItemInvoked));

        private void OnItemInvoked(NavigationViewItemInvokedEventArgs args)
        {
            // could also use a converter on the command parameter if you don't like
            // the idea of passing in a NavigationViewItemInvokedEventArgs
            if (args.InvokedItem.ToString().Equals("Statistics"))
            {
                ChildPageNavigation.ViewModel = new StatisticsViewModel();
            }
            else if (args.InvokedItem.ToString().Equals("Booking Schedule"))
            {
                ChildPageNavigation.ViewModel = new BookingScheduleViewModel();
            }
            else if (args.InvokedItem.ToString().Equals("Manage Customer"))
            {
                ChildPageNavigation.ViewModel = new ManageCustomerViewModel();
            }
            else if (args.InvokedItem.ToString().Equals("Manage Driver"))
            {
                ChildPageNavigation.ViewModel = new ManageDriverViewModel();
            }
            else if (args.InvokedItem.ToString().Equals("Logs"))
            {
                ChildPageNavigation.ViewModel = new LogsViewModel();
            }

        }
        public PageNavigation ChildPageNavigation { get; set; }
        //public Account Account { get => _account; set => _account = value; }
    }
}