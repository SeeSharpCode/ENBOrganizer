using ENBOrganizer.App.Messages;
using ENBOrganizer.Domain.Entities;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

namespace ENBOrganizer.App.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly ViewModelLocator _viewModelLocator;

        private bool _isAddGameFlyoutOpen;

        public bool IsAddGameFlyoutOpen
        {
            get { return _isAddGameFlyoutOpen; }
            set { Set("IsAddGameFlyoutOpen", ref _isAddGameFlyoutOpen, value); }
        }

        private ViewModelBase _currentDialogViewModel;

        public ViewModelBase CurrentDialogViewModel
        {
            get { return _currentDialogViewModel; }
            set { Set("CurrentDialogViewModel", ref _currentDialogViewModel, value); }
        }
        
        private ViewModelBase _currentPresetViewModel;

        public ViewModelBase CurrentPresetViewModel
        {
            get { return _currentPresetViewModel; }
            set { Set("CurrentPresetViewModel", ref _currentPresetViewModel, value); }
        }

        public MainViewModel(ViewModelLocator viewModelLocator)
        {
            _viewModelLocator = viewModelLocator;

            CurrentPresetViewModel = _viewModelLocator.PresetsOverviewViewModel;

            MessengerInstance.Register<PropertyChangedMessage<Game>>(this, (message) => CurrentPresetViewModel = _viewModelLocator.PresetsOverviewViewModel);
            MessengerInstance.Register<NavigationMessage>(this, true, OnNavigationMessage);
            MessengerInstance.Register<DialogMessage>(this, true, (message) => OnDialogMessage(message));
        }

        private void OnNavigationMessage(NavigationMessage navigationMessage)
        {
            switch (navigationMessage.ViewName)
            {
                case ViewName.PresetDetail:
                    CurrentPresetViewModel = _viewModelLocator.PresetDetailViewModel;
                    break;
                case ViewName.PresetsOverview:
                    CurrentPresetViewModel = _viewModelLocator.PresetsOverviewViewModel;
                    break;
                default:
                    break;
            }
        }

        private void OnDialogMessage(DialogMessage dialogMessage)
        {
            if (dialogMessage.DialogAction == DialogAction.Open)
            {
                IsAddGameFlyoutOpen = true;

                OpenDialogMessage openDialogMessage = dialogMessage as OpenDialogMessage;

                if (openDialogMessage.Dialog == Dialog.AddGame)
                    CurrentDialogViewModel = _viewModelLocator.AddGameViewModel;
                else
                    CurrentDialogViewModel = _viewModelLocator.AddMasterListItemViewModel;
            }
            else
                IsAddGameFlyoutOpen = false;
        }
    }
}
