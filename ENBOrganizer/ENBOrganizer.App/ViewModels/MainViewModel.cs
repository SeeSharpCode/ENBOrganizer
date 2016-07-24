using ENBOrganizer.App.Messages;
using ENBOrganizer.Domain.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.Generic;
using System.Windows.Input;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace ENBOrganizer.App.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly ViewModelLocator _viewModelLocator;
        private readonly GameService _gameService;
        private readonly DialogService _dialogService;
        private readonly MasterListService _masterListService;

        public SettingsService SettingsService { get; set; }
        public List<IPageViewModel> PageViewModels { get; set; }
        public ICommand OpenENBBinariesLinkCommand { get; set; }
        public ICommand OpenNexusLinkCommand { get; set; }
        public ICommand OpenGitHubLinkCommand { get; set; }
        public ICommand OpenAboutDialogCommand { get; set; }

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

        public MainViewModel(GameService gameService, SettingsService settingsService, DialogService dialogService, MasterListService masterListService)
        {
            _viewModelLocator = (ViewModelLocator)App.Current.Resources["ViewModelLocator"];
            _gameService = gameService;
            SettingsService = settingsService;
            _dialogService = dialogService;
            _masterListService = masterListService;

            PageViewModels = new List<IPageViewModel>()
            {
                _viewModelLocator.GamesViewModel,
                _viewModelLocator.BinariesViewModel,
                _viewModelLocator.PresetsViewModel,
                _viewModelLocator.MasterListViewModel
            };

            CurrentPageViewModel = _viewModelLocator.GamesViewModel;

            OpenENBBinariesLinkCommand = new RelayCommand(OpenENBBinariesLink);
            OpenNexusLinkCommand = new RelayCommand(OpenNexusLink);
            OpenGitHubLinkCommand = new RelayCommand(OpenGitHubLink);
            OpenAboutDialogCommand = new RelayCommand(OpenAboutDialog);

            MessengerInstance.Register<DialogMessage>(this, OnDialogMessage);

            InitializeApplication();
        }

        private void OpenAboutDialog()
        {
            _dialogService.ShowInfoDialog("ENB Organizer v" + Assembly.GetExecutingAssembly().GetName().Version.ToString() +
                Environment.NewLine + "By Breems");
        }

        private void InitializeApplication()
        {
            UpgradeSettings();
            AddGamesFromRegistry();
            AddDefaultMasterListItems();     
            CheckForUpdate();
        }

        private void UpgradeSettings()
        {
            try
            {
                SettingsService.UpgradeSettings();
            }
            catch (Exception exception)
            {
                _dialogService.ShowErrorDialog("Error upgrading settings." + Environment.NewLine + exception.Message);
            }
        }

        private void AddDefaultMasterListItems()
        {
            try
            {
                if (!_masterListService.Items.Any())
                    _masterListService.AddDefaultItems();
            }
            catch (Exception exception)
            {
                _dialogService.ShowErrorDialog("Error creating default master list items. This is not a critical error, although adding a preset via Import Installed Files " 
                    + "may not pick up all desired files/folders until you start adding presets." + Environment.NewLine + Environment.NewLine + exception.Message);
            }
        }

        private void AddGamesFromRegistry()
        {
            try
            {
                if (!SettingsService.FirstUse)
                    return;

                SettingsService.FirstUse = false;
                _gameService.AddGamesFromRegistry();
            }
            // Non-critical process, don't inform the user.
            catch (Exception) { }
        }

        private async void CheckForUpdate()
        {
            try
            {
                UpdateAvailable = await UpdateService.IsUpdateAvailable();
            }
            catch (Exception exception)
            {
                UpdateAvailable = false;

                _dialogService.ShowWarningDialog("Failed to check for update. If this error persists, check the ENB Organizer Nexus page for updates."
                    + Environment.NewLine + Environment.NewLine + exception.Message);
            }
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
                case DialogName.GlobalEnbLocal:
                    CurrentDialogViewModel = _viewModelLocator.GlobalEnbLocalViewModel;
                    break;
                case DialogName.AddENBoostPreset:
                    CurrentDialogViewModel = _viewModelLocator.AddENBoostPresetViewModel;
                    break;
            }

            IsDialogOpen = message.DialogAction == DialogAction.Open;
        }
    }
}