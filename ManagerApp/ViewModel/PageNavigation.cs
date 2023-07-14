using CommunityToolkit.Mvvm.ComponentModel;

namespace ManagerApp.ViewModel
{
    public class PageNavigation : ObservableObject
    {
        private ViewModelBase _viewModel;

        public PageNavigation(ViewModelBase viewModel)
        {
            ViewModel = viewModel;
        }

        public ViewModelBase ViewModel
        {
            get => _viewModel;
            set
            {
                ChangeViewModel(value);
            }
        }

        private void ChangeViewModel(ViewModelBase value)
        {
            SetProperty(ref _viewModel, value, nameof(ViewModel));
            _viewModel.ParentPageNavigation = this;
        }
    }
}
