using ENBOrganizer.App.Messages;
using ENBOrganizer.Domain.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.Generic;
using System.Windows.Input;
using System;
using System.Diagnostics;

namespace ENBOrganizer.App.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly ViewModelLocator _viewModelLocator;
        private readonly GameService _gameService;
        private readonly DialogService _dialogService;

        public SettingsService SettingsService { get; set; }
        public List<IPageViewModel> PageViewModels { get; set; }
        public ICommand OpenENBBinariesLinkCommand { get; set; }
        public ICommand OpenNexusLinkCommand { get; set; }
        public ICommand OpenGitHubLinkCommand { get; set; }

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

        private bool _updateAvailable;

        public bool UpdateAvailable
        {
            get { return _updateAvailable; }
            set { Set(nameof(UpdateAvailable), ref _updateAvailable, value); }
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

        public MainViewModel(GameService gameService, SettingsService settingsService, DialogService dialogService)
        {
            _viewModelLocator = (ViewModelLocator)App.Current.Resources["ViewModelLocator"];
            _gameService = gameService;
            SettingsService = settingsService;
            _dialogService = dialogService;

            PageViewModels = new List<IPageViewModel>()
            {
                _viewModelLocator.GamesViewModel,
                _viewModelLocator.BinariesViewModel,
                _viewModelLocator.PresetsViewModel,
                _viewModelLocator.MasterListViewModel,
            };

            CurrentPageViewModel = _viewModelLocator.GamesViewModel;

            OpenENBBinariesLinkCommand = new RelayCommand(OpenENBBinariesLink);
            OpenNexusLinkCommand = new RelayCommand(OpenNexusLink);
            OpenGitHubLinkCommand = new RelayCommand(OpenGitHubLink);

            MessengerInstance.Register<DialogMessage>(this, OnDialogMessage);

            SettingsService.InitializeSettings();

            CheckForUpdate();
        }

        private async void CheckForUpdate()
        {
            UpdateAvailable = await UpdateService.IsUpdateAvailable();
        }

        private void OpenGitHubLink()
        {
            try
            {
                Process.Start("https://github.com/SeeSharpCode/ENBOrganizer");
            }
            catch (Exception)
            {
                _dialogService.ShowErrorDialog("Unable to open GitHub page. Visit https://github.com/SeeSharpCode/ENBOrganizer in your browser.");
            }
        }

        private void OpenNexusLink()
        {
            try
            {
                Process.Start("http://www.nexusmods.com/skyrim/mods/67077");
            }
            catch (Exception)
            {
                _dialogService.ShowErrorDialog("Unable to open Nexus page. Visit http://www.nexusmods.com/skyrim/mods/67077 in your browser.");
            }
        }

        private void OpenENBBinariesLink()
        {
            try
            {
                Process.Start("http://enbdev.com/download.htm");
            }
            catch (Exception)
            {
                _dialogService.ShowErrorDialog("Unable to open ENB page. Visit http://enbdev.com/download.htm in your browser.");
            }
        }

        private void OnDialogMessage(DialogMessage message)
        {
            switch (message.DialogName)
            {
                case DialogName.AddBinary:
                    CurrentDialogViewModel = _viewModelLocator.AddBinaryViewModel;
                    break;
                case DialogName.GameDetail:
                    CurrentDialogViewModel = _viewModelLocator.GameDetailViewModel;
                    break;
                case DialogName.AddMasterListItem:
                    CurrentDialogViewModel = _viewModelLocator.AddMasterListItemViewModel;
                    break;
                case DialogName.AddPreset:
                    CurrentDialogViewModel = _viewModelLocator.AddPresetViewModel;
                    break;
                case DialogName.EditPreset:
                    CurrentDialogViewModel = _viewModelLocator.EditPresetViewModel;
                    break;
            }

            IsDialogOpen = message.DialogAction == DialogAction.Open;
        }
    }
}