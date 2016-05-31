using ENBOrganizer.App.Messages;
using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.IO;
using System.Windows.Input;

namespace ENBOrganizer.App.ViewModels
{
    // TODO: fix editing games
    public class GameDetailViewModel : ViewModelBase
    {
        private readonly GameService _gameService;
        private readonly DialogService _dialogService;
        private Game _game;

        public ICommand BrowseCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { Set(nameof(Name), ref _name, value); }
        }

        private string _executablePath;

        public string ExecutablePath
        {
            get { return _executablePath; }
            set { Set(nameof(ExecutablePath), ref _executablePath, value); }
        }
        
        public GameDetailViewModel(GameService gameService, PresetService presetService, DialogService dialogService)
        {
            _gameService = gameService;
            _dialogService = dialogService;

            _game = new Game();

            MessengerInstance.Register<Game>(this, OnGameReceived);

            BrowseCommand = new RelayCommand(BrowseForGameFile);
            SaveCommand = new RelayCommand(Save, CanSave);
            CancelCommand = new RelayCommand(Close);
        }

        private void OnGameReceived(Game game)
        {
            _game = game;

            Name = game.Name;
            ExecutablePath = game.ExecutablePath;
        }

        private void Close()
        {
            _game = new Game();

            _dialogService.CloseDialog(DialogName.AddGame);
        }

        private void Save()
        {
            try
            {
                _game.Name = Name;
                _game.ExecutablePath = ExecutablePath;

                if (string.IsNullOrEmpty(_game.ID))
                    _gameService.Add(_game);
                else
                    _gameService.SaveChanges();
            }
            catch (Exception exception) 
            {
                _dialogService.ShowErrorDialog(exception.Message);
            }
            finally
            {
                Close();
            }
        }

        private bool CanSave()
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
