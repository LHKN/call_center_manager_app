using CommunityToolkit.Mvvm.ComponentModel;

namespace ManagerApp.ViewModel
{
    internal class RootViewModel : ObservableObject
    {
        public RootViewModel()
        {
            //ChildPageNavigation = new PageNavigation(new LoginDatabaseViewModel());
            ChildPageNavigation = new PageNavigation(new HomeViewModel());
        }
        public PageNavigation ChildPageNavigation { get; }
    }
}
