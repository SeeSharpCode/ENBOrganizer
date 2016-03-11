using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;
using ENBOrganizer.Domain.Services;
using ENBOrganizer.Model.Entities;
using ENBOrganizer.Util;
using Microsoft.Win32;

namespace ENBOrganizer.UI.ViewModels
{
    public class ManageGamesViewModel : ObservableObject, IDataErrorInfo 
    {
        private readonly GameService _gameService;
        private Game _selectedGame;
        private Game _game;

        public GamesViewModel GamesViewModel { get; set; }
        public ICommand DeleteGameCommand { get; set; }
        public ICommand AddGameCommand { get; set; }
        public ICommand BrowseCommand { get; set; }

        public Game SelectedGame
        {
            get { return _selectedGame; }
            set
            {
                _selectedGame = value;

                if (value == null)
                    return;

                Name = value.Name;
                ExePath = value.ExecutablePath;
            }
        }

        public string Name
        {
            get { return _game.Name; }
            set
            {
                _game.Name = value.Trim();
                RaisePropertyChanged("Name");
            }
        }

        public string ExePath
        {
            get { return _game.ExecutablePath; }
            set
            {
                _game.ExecutablePath = value.Trim();
                RaisePropertyChanged("ExePath");
            }
        }
        
        public string Error
        {
            get { return null; } 
        }

        // TODO: (UI) only validate control when trying to add new item
        public string this[string columnName]
        {
            get
            {
                string errorMessage = string.Empty;

                switch (columnName)
                {
                    case "Name":
                        if (string.IsNullOrEmpty(_game.Name))
                            errorMessage = "Name is required";
                        break;
                    case "ExePath":
                        if (string.IsNullOrEmpty(_game.ExecutablePath))
                            errorMessage = "Invalid file path";
                        break;
                }
                    
                return errorMessage;
            }
        }

        public ManageGamesViewModel()
            : this(ViewModelSingletons.GamesViewModel, ServiceSingletons.GameService)
        { }

        private ManageGamesViewModel(GamesViewModel gamesViewModel, GameService gameService)
        {
            _gameService = gameService;
            _game = new Game();

            GamesViewModel = gamesViewModel;

            DeleteGameCommand = new ActionCommand(DeleteGame, CanDelete);
            AddGameCommand = new ActionCommand(AddGame, CanAdd);
            BrowseCommand = new ActionCommand(BrowseForGameFile, () => true);
        }
        
        private void AddGame()
        {
            try
            {
                _gameService.Add(_game);

                // Reset the game object to avoid duplicate games.
                _game = new Game(); 
            }
            catch (InvalidOperationException exception)
            {
                MessageBoxUtil.ShowError(exception.Message);
            }
        }

        private bool CanAdd()
        {
            return !string.IsNullOrEmpty(_game.Name) && !string.IsNullOrEmpty(_game.ExecutablePath);
        }

        private void BrowseForGameFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "EXE Files (*.exe)|*.exe",
                Title = "Select the game's .exe file"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                Name = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
                ExePath = openFileDialog.FileName;
            }
        }

        private void DeleteGame()
        {
            try
            {
                _gameService.Delete(_selectedGame);
            }
            catch (Exception exception)
            {
                MessageBoxUtil.ShowError(exception.Message);
            }
        }

        private bool CanDelete()
        {
            return _selectedGame != null;
        }
    }
}