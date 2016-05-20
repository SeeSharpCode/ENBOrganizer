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
    public class AddGameViewModel : ViewModelBase
    {
        private readonly GameService _gameService;
        private readonly DialogService _dialogService;
        
        public ICommand BrowseCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        private Game _game;

        public Game Game
        {
            get { return _game; }
            set { Set(nameof(Game), ref _game, value); }
        }

        public AddGameViewModel(GameService gameService, PresetService presetService, DialogService dialogService)
        {
            _gameService = gameService;
            _dialogService = dialogService;

            Game = new Game();

            MessengerInstance.Register<Game>(this, game => Game = game);

            BrowseCommand = new RelayCommand(BrowseForGameFile);
            SaveCommand = new RelayCommand(Save, CanSave);
            CancelCommand = new RelayCommand(Close);
        }

        private void Close()
        {
            Game = new Game();

            _dialogService.CloseDialog(DialogName.AddGame);
        }

        private void Save()
        {
            try
            {
                if (string.IsNullOrEmpty(Game.ID))
                    _gameService.Add(Game);
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
            return !string.IsNullOrWhiteSpace(Game.Name) && !string.IsNullOrWhiteSpace(Game.ExecutablePath) && File.Exists(Game.ExecutablePath);
        }

        private void BrowseForGameFile()
        {
            string gameFilePath = _dialogService.PromptForFile("Select the game's .exe file", "EXE Files (*.exe)|*.exe");

            if (string.IsNullOrWhiteSpace(gameFilePath))
                return;

            Game.Name = Path.GetFileNameWithoutExtension(gameFilePath);
            Game.ExecutablePath = gameFilePath;
        }
    }
}
