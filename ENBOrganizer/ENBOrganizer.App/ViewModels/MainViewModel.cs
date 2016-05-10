using ENBOrganizer.App.Messages;
using GalaSoft.MvvmLight;

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

        public MainViewModel(ViewModelLocator viewModelLocator)
        {
            _viewModelLocator = viewModelLocator;
            
            MessengerInstance.Register<DialogMessage>(this, true, (message) => OnDialogMessage(message));
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
