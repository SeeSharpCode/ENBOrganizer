using ENBOrganizer.App.Messages;
using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Services;
using GalaSoft.MvvmLight.CommandWpf;
using MvvmValidation;
using System;
using System.IO;
using System.Windows.Input;

namespace ENBOrganizer.App.ViewModels.Games
{
    public class GameDetailViewModel : DialogViewModelBase
    {
        private readonly GameService _gameService;
        private Game _game;

        public ICommand BrowseCommand { get; set; }

        private string _executablePath;

        // TODO: this isn't validating at first
        public string ExecutablePath
        {
            get { return _executablePath; }
            set { Set(nameof(ExecutablePath), ref _executablePath, value); _validator.Validate(() => ExecutablePath); }
        }

        public GameDetailViewModel(GameService gameService)
        {
            _gameService = gameService;

            _game = new Game();

            MessengerInstance.Register<Game>(this, OnGameReceived);

            BrowseCommand = new RelayCommand(BrowseForGameFile);
            
            _validator.AddRequiredRule(() => Name, "Name is totes required.");
            _validator.AddRequiredRule(() => ExecutablePath, "Exe path is totes required.");

            _validator.ValidateAll();
        }

        private void OnGameReceived(Game game)
        {
            _game = game;

            Name = game.Name;
            ExecutablePath = game.ExecutablePath;
        }

        protected override void Close()
        {
            _game = new Game();

            _dialogService.CloseDialog(DialogName.GameDetail);
        }

        protected override void Save()
        {
            try
            {
                _game.Name = Name.Trim();
                _game.ExecutablePath = ExecutablePath.Trim();

                if (string.IsNullOrWhiteSpace(_game.ID))
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

        protected override bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(ExecutablePath) && File.Exists(ExecutablePath);
        }

        private void BrowseForGameFile()
        {
            string gameFilePath = _dialogService.ShowOpenFileDialog("Select the game's .exe file", "EXE Files (*.exe)|*.exe");

            if (string.IsNullOrWhiteSpace(gameFilePath))
                return;

            Name = Path.GetFileNameWithoutExtension(gameFilePath);
            ExecutablePath = gameFilePath;
        }
    }
}
