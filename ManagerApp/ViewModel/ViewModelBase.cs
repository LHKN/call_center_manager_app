using CommunityToolkit.Mvvm.ComponentModel;

namespace ManagerApp.ViewModel
{
    public class ViewModelBase: ObservableObject
    {
        public PageNavigation ParentPageNavigation { get; set; }
    }
}
