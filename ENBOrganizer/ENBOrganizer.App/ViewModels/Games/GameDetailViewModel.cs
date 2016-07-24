using ENBOrganizer.App.Messages;
using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Exceptions;
using ENBOrganizer.Domain.Services;
using ENBOrganizer.Util;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;

namespace ENBOrganizer.App.ViewModels.Games
{
    public class GameDetailViewModel : DialogViewModelBase
    {
        private readonly GameService _gameService;
        private Game _existingGame;

        public ICommand BrowseCommand { get; set; }

        private string _executablePath;

        public string ExecutablePath
        {
            get { return _executablePath; }
            set { Set(nameof(ExecutablePath), ref _executablePath, value); }
        }

        public GameDetailViewModel(GameService gameService)
        {
            _gameService = gameService;

            MessengerInstance.Register<Game>(this, OnGameReceived);

            BrowseCommand = new RelayCommand(BrowseForGameFile);

            ValidatedProperties = new List<string>
            {
                nameof(Name),
                nameof(ExecutablePath)
            };
        }

        private void OnGameReceived(Game game)
        {
            _existingGame = game;

            Name = game.Name;
            ExecutablePath = game.ExecutablePath;
        }

        protected override void Close()
        {
            _existingGame = null;

            Name = string.Empty;
            ExecutablePath = string.Empty;

            _dialogService.CloseDialog(DialogName.GameDetail);
        }

        protected override void Save()
        {
            try
            {
                Game newGame = new Game(Name.Trim(), ExecutablePath.Trim());

                if (_existingGame == null)
                    _gameService.Add(newGame);
                else
                    EditGame(newGame);
            }
            catch (DuplicateEntityException)
            {
                _dialogService.ShowErrorDialog("Can't save this game because a game already exists with this name or directory.");
            }
            catch (Exception exception)
            {
                _dialogService.ShowErrorDialog("Error saving game." + Environment.NewLine + exception.Message);
            }
            finally
            {
                Close();
            }
        }

        private void EditGame(Game newGame)
        {
            if (!ShouldEdit(newGame))
                return;

            bool updateCurrentGame = _settingsService.CurrentGame.Equals(_existingGame);

            if (!_existingGame.Name.EqualsIgnoreCase(newGame.Name))
                _gameService.Rename(_existingGame, newGame);

            _existingGame.Name = newGame.Name;
            _existingGame.ExecutablePath = newGame.ExecutablePath;

            _gameService.SaveChanges();

            if (updateCurrentGame)
                _settingsService.CurrentGame = _existingGame;
        }

        private bool ShouldEdit(Game newGame)
        {
            if (_existingGame.Equals(newGame))
                return false;

            if (_gameService.Items.Contains(newGame))
            {
                _dialogService.ShowErrorDialog("This game already exists.");
                return false;
            }

            return true;
        }

        private void BrowseForGameFile()
        {
            string gameFilePath = _dialogService.ShowOpenFileDialog("Select the game's .exe file", "EXE Files (*.exe)|*.exe");

            if (string.IsNullOrWhiteSpace(gameFilePath))
                return;

            Name = Path.GetFileNameWithoutExtension(gameFilePath);
            ExecutablePath = gameFilePath;
        }

        protected override string GetValidationError(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Name):
                    return ValidateFileSystemName();
                case nameof(ExecutablePath):
                    return File.Exists(ExecutablePath) ? string.Empty : "File does not exist.";
            }

            return string.Empty;
        }
    }
}
