using ENBOrganizer.App.Messages;
using GalaSoft.MvvmLight;

namespace ENBOrganizer.App.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly ViewModelLocator _viewModelLocator;

        public GamesViewModel GamesViewModel { get { return _viewModelLocator.GamesViewModel; } }

        private bool _isAddGameFlyoutOpen;

        public bool IsAddGameFlyoutOpen
        {
            get { return _isAddGameFlyoutOpen; }
            set { Set(nameof(IsAddGameFlyoutOpen), ref _isAddGameFlyoutOpen, value); }
        }

        private ViewModelBase _currentDialogViewModel;

        public ViewModelBase CurrentDialogViewModel
        {
            get { return _currentDialogViewModel; }
            set { Set(nameof(CurrentDialogViewModel), ref _currentDialogViewModel, value); }
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

                switch (openDialogMessage.Dialog)
                {
                    case Dialog.AddGame:
                        CurrentDialogViewModel = _viewModelLocator.AddGameViewModel;
                        break;
                    case Dialog.AddMasterListItem:
                        CurrentDialogViewModel = _viewModelLocator.AddMasterListItemViewModel;
                        break;
                    case Dialog.ImportPreset:
                        CurrentDialogViewModel = _viewModelLocator.ImportPresetViewModel;
                        break;
                }
            }
            else
                IsAddGameFlyoutOpen = false;
        }
    }
}
