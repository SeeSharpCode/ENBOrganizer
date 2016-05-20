using ENBOrganizer.App.Messages;
using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
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

        public AddGameViewModel(GameService gameService, PresetService presetService, DialogService dialogService)
        {
            _gameService = gameService;
            _dialogService = dialogService;

            MessengerInstance.Register<Game>(this, OnUpdateGameMessageReceived);

            BrowseCommand = new RelayCommand(BrowseForGameFile);
            SaveCommand = new RelayCommand(Save, CanSave);
            CancelCommand = new RelayCommand(Close);
        }

        private void OnUpdateGameMessageReceived(Game game)
        {
            _game = game;

            Name = game.Name;
            ExecutablePath = game.ExecutablePath;
        }

        private void Close()
        {
            _game = null;

            _dialogService.CloseDialog(DialogName.AddGame);
        }

        private void Save()
        {
            try
            {
                if (_game == null)
                    _gameService.Add(new Game(Name, ExecutablePath));
                else
                    UpdateGame();
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

        private void UpdateGame()
        {
            _game.Name = Name;
            _game.ExecutablePath = ExecutablePath;

            _gameService.SaveChanges();

            foreach (Preset preset in _game.Presets)
                preset.Game = _game;

            SimpleIoc.Default.GetInstance<PresetService>().SaveChanges();
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
