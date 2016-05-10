using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Exceptions;
using ENBOrganizer.Domain.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.IO;
using System.Windows.Input;

namespace ENBOrganizer.App.ViewModels
{
    public class AddGameViewModel : ViewModelBase
    {
        private readonly GameService _gameService;
        private readonly GamesViewModel _gamesViewModel;
        private readonly DialogService _dialogService;

        public ICommand BrowseCommand { get; set; }
        public ICommand AddGameCommand { get; set; }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { Set(nameof(Name), ref _name, value.Trim()); }
        }

        private string _executablePath;

        public string ExecutablePath
        {
            get { return _executablePath; }
            set { Set(nameof(ExecutablePath), ref _executablePath, value); }
        }

        public AddGameViewModel(GameService gameService, GamesViewModel gamesViewModel, DialogService dialogService)
        {
            _gameService = gameService;
            _gamesViewModel = gamesViewModel;
            _dialogService = dialogService;

            BrowseCommand = new RelayCommand(BrowseForGameFile);
            AddGameCommand = new RelayCommand(AddGame, CanAdd);
        }

        private async void AddGame()
        {
            try
            {
                _gameService.Add(new Game(Name, ExecutablePath));
            }
            catch (Exception exception) 
            {
                await _dialogService.ShowErrorDialog(exception.Message);
            }
            finally
            {
                Name = string.Empty;
                ExecutablePath = string.Empty;

                _dialogService.CloseDialog();
            }
        }

        private bool CanAdd()
        {
            return !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(ExecutablePath) && File.Exists(ExecutablePath);
        }

        private void BrowseForGameFile()
        {
            string gameFilePath = _dialogService.PromptForFile("Select the game's .exe file", "EXE Files (*.exe)|*.exe");

            if (string.IsNullOrWhiteSpace(gameFilePath))
                return;

            Name = Path.GetFileNameWithoutExtension(gameFilePath);
            ExecutablePath = gameFilePath;
        }
    }
}
