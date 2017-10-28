using FriendOrganizer.Model;
using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.Event;
using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace FriendOrganizer.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private IEventAggregator _eventAggregator;
        private IFriendDetailViewModel friendDetailViewModel;
        private Func<IFriendDetailViewModel> _friendDetailViewModelCreator;

        public MainViewModel(INavigationViewModel navigationViewModel,
            Func<IFriendDetailViewModel> friendDetailViewModelCreator,
            IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _friendDetailViewModelCreator = friendDetailViewModelCreator;

            _eventAggregator.GetEvent<OpenFriendDetailViewEvent>()
                .Subscribe(OnOpenFriendDetailView);

            NavigationViewModel = navigationViewModel;
        }

        public async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();
        }

        public INavigationViewModel NavigationViewModel { get; }        

        public IFriendDetailViewModel FriendDetailViewModel
        {
            get { return friendDetailViewModel; }
            private set
            {
                friendDetailViewModel = value;
                OnPropertyChanged();
            }
        }

        private async void OnOpenFriendDetailView(int friendId)
        {
            if(FriendDetailViewModel != null && FriendDetailViewModel.HasChanges)
            {
                var result = MessageBox.Show("You've made changes. Navigate away?", "Question",
                    MessageBoxButton.OKCancel);

                if (result == MessageBoxResult.Cancel)
                {
                    return;
                }
            }
            FriendDetailViewModel = _friendDetailViewModelCreator();
            await FriendDetailViewModel.LoadAsync(friendId);
        }
    }
}
