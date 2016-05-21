using GalaSoft.MvvmLight;
using System.Collections.Generic;

namespace ENBOrganizer.App.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly ViewModelLocator _viewModelLocator;
        
        public List<IPageViewModel> PageViewModels { get; set; }

        private IPageViewModel _selectedViewModel;

        public IPageViewModel SelectedViewModel
        {
            get { return _selectedViewModel; }
            set
            {
                Set(nameof(SelectedViewModel), ref _selectedViewModel, value);
                IsMenuToggleChecked = false;
            }
        }

        private bool _isMenuToggleChecked;

        public bool IsMenuToggleChecked
        {
            get { return _isMenuToggleChecked; }
            set { Set(nameof(IsMenuToggleChecked), ref _isMenuToggleChecked, value); }
        }
        
        public MainViewModel(ViewModelLocator viewModelLocator)
        {
            _viewModelLocator = viewModelLocator;

            PageViewModels = new List<IPageViewModel>
            {
                _viewModelLocator.GamesViewModel,
                _viewModelLocator.BinariesViewModel,
                _viewModelLocator.MasterListViewModel
            };

            SelectedViewModel = _viewModelLocator.GamesViewModel;
        }
    }
}
