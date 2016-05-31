using ENBOrganizer.App.Messages;
using GalaSoft.MvvmLight;
using System.Collections.Generic;

namespace ENBOrganizer.App.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly ViewModelLocator _viewModelLocator;
        
        public List<IPageViewModel> PageViewModels { get; set; }

        private IPageViewModel _currentPageViewModel;

        public IPageViewModel CurrentPageViewModel
        {
            get { return _currentPageViewModel; }
            set
            {
                Set(nameof(CurrentPageViewModel), ref _currentPageViewModel, value);
                IsMenuToggleChecked = false;
            }
        }

        private bool _isDialogOpen;

        public bool IsDialogOpen
        {
            get { return _isDialogOpen; }
            set { Set(nameof(IsDialogOpen), ref _isDialogOpen, value); }
        }

        private DialogViewModelBase _currentDialogViewModel;

        public DialogViewModelBase CurrentDialogViewModel
        {
            get { return _currentDialogViewModel; }
            set { Set(nameof(CurrentDialogViewModel), ref _currentDialogViewModel, value); }
        }
        
        private bool _isMenuToggleChecked;

        public bool IsMenuToggleChecked
        {
            get { return _isMenuToggleChecked; }
            set { Set(nameof(IsMenuToggleChecked), ref _isMenuToggleChecked, value); }
        }
        
        public MainViewModel()
        {
            _viewModelLocator = (ViewModelLocator)App.Current.Resources["ViewModelLocator"];

            PageViewModels = new List<IPageViewModel>()
            {
                _viewModelLocator.GamesViewModel,
                _viewModelLocator.BinariesViewModel,
                _viewModelLocator.MasterListViewModel
            };

            CurrentPageViewModel = _viewModelLocator.GamesViewModel;

            MessengerInstance.Register<DialogMessage>(this, OnDialogMessage);
        }

        private void OnDialogMessage(DialogMessage message)
        {
            // TODO: switch on DialogName and set CurrentDialogViewModel accordingly
            //CurrentDialogViewModel = message.DialogViewModel;
            IsDialogOpen = message.DialogAction == DialogAction.Open;
        }
    }
}
