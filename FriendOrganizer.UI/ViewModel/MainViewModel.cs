﻿using FriendOrganizer.UI.Event;
using FriendOrganizer.UI.View.Services;
using Prism.Commands;
using Prism.Events;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FriendOrganizer.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private IEventAggregator _eventAggregator;
        private IDetailViewModel _detailViewModel;
        private Func<IFriendDetailViewModel> _friendDetailViewModelCreator;
        private IMessageDialogService _messageDialogService;

        public MainViewModel(INavigationViewModel navigationViewModel,
            Func<IFriendDetailViewModel> friendDetailViewModelCreator,
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService)
        {
            _eventAggregator = eventAggregator;
            _friendDetailViewModelCreator = friendDetailViewModelCreator;
            _messageDialogService = messageDialogService;

            _eventAggregator.GetEvent<OpenDetailViewEvent>()
                .Subscribe(OnOpenDetailView);
            _eventAggregator.GetEvent<AfterDetailDeletedEvent>()
                .Subscribe(AfterDetailDeleted);

            CreateNewFriendCommand = new DelegateCommand(OnCreateNewFriendExecute);

            NavigationViewModel = navigationViewModel;
        }

        public async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();
        }

        public ICommand CreateNewFriendCommand { get; }

        public INavigationViewModel NavigationViewModel { get; }

        public IDetailViewModel DetailViewModel
        {
            get { return _detailViewModel; }
            private set
            {
                _detailViewModel = value;
                OnPropertyChanged();
            }
        }

        private async void OnOpenDetailView(OpenDetailViewEventArgs args)
        {
            if (DetailViewModel != null && DetailViewModel.HasChanges)
            {
                var result = _messageDialogService.ShowOkCancelDialog("You've made changes. Navigate away?", "Question");
                if (result == MessageDialogResult.Cancel)
                {
                    return;
                }
            }

            switch (args.ViewModelName)
            {
                case nameof(FriendDetailViewModel):
                    DetailViewModel = _friendDetailViewModelCreator();
                    break;
            }

            await DetailViewModel.LoadAsync(args.Id);
        }

        private void OnCreateNewFriendExecute()
        {
            OnOpenDetailView(null);
        }

        private void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            DetailViewModel = null;
        }
    }
}
