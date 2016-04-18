using ENBOrganizer.App.Messages;
using GalaSoft.MvvmLight;

namespace ENBOrganizer.App.ViewModels
{
    // TODO: fix add game with messaging
    public class MainViewModel : ViewModelBase
    {
        private PresetsOverviewViewModel _presetsOverviewViewModel;
        private PresetDetailViewModel _presetDetailViewModel;

        private bool _isAddGameFlyoutOpen;

        public bool IsAddGameFlyoutOpen
        {
            get { return _isAddGameFlyoutOpen; }
            set
            {
                _isAddGameFlyoutOpen = value;
                RaisePropertyChanged("IsAddGameFlyoutOpen");
            }
        }
        
        private ViewModelBase _currentViewModel;

        public ViewModelBase CurrentViewModel
        {
            get { return _currentViewModel; }
            set
            {
                _currentViewModel = value;
                RaisePropertyChanged("CurrentViewModel");
            }
        }

        public MainViewModel(PresetsOverviewViewModel presetsOverviewViewModel, PresetDetailViewModel presetDetailViewModel)
        {
            _presetsOverviewViewModel = presetsOverviewViewModel;
            _presetDetailViewModel = presetDetailViewModel;

            CurrentViewModel = _presetsOverviewViewModel;
            
            MessengerInstance.Register<NavigationMessage>(this, true, (message) => OnNavigationMessage(message));
            MessengerInstance.Register<DialogMessage>(this, (message) => OnDialogMessage(message));
        }

        private void OnNavigationMessage(NavigationMessage navigationMessage)
        {
            switch (navigationMessage.ViewName)
            {
                case ViewNames.PresetDetail:
                    _presetDetailViewModel.Preset = ((PresetDetailNavigationMessage)navigationMessage).Preset;
                    CurrentViewModel = _presetDetailViewModel;
                    break;
                case ViewNames.PresetsOverview:
                    CurrentViewModel = _presetsOverviewViewModel;
                    break;
                default:
                    break;
            }
        }

        private void OnDialogMessage(DialogMessage dialogMessage)
        {
            IsAddGameFlyoutOpen = dialogMessage.DialogAction == DialogActions.Open;
        }
    }
}
