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

            // TODO: fix 
            MessengerInstance.Register<NavigationMessage>(this, (message) => CurrentViewModel = _presetsOverviewViewModel);
            MessengerInstance.Register<GoToPresetDetailMessage>(this, GoToPresetDetailView);
        }

        private void GoToPresetDetailView(GoToPresetDetailMessage message)
        {
            _presetDetailViewModel.Preset = message.Preset;
            CurrentViewModel = _presetDetailViewModel;
        }
    }
}
