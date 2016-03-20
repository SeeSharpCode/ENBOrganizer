using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;
using ENBOrganizer.Domain.Services;
using ENBOrganizer.Model.Entities;
using ENBOrganizer.Util;
using Microsoft.Win32;
using ENBOrganizer.Util.UI;

namespace ENBOrganizer.UI.ViewModels
{
    public class ManageGamesViewModel : ObservableObject, IDataErrorInfo 
    {
        private readonly GameService _gameService;
        private readonly PresetService _presetService;
        private Game _selectedGame;

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
                ExecutablePath = value.ExecutablePath;
            }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value.Trim();
                RaisePropertyChanged("Name");
            }
        }

        private string _executablePath;

        public string ExecutablePath
        {
            get { return _executablePath; }
            set
            {
                _executablePath = value;
                RaisePropertyChanged("ExecutablePath");
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
                        if (string.IsNullOrEmpty(Name))
                            errorMessage = "Name is required";
                        break;
                    case "ExePath":
                        if (string.IsNullOrEmpty(ExecutablePath))
                            errorMessage = "Invalid file path";
                        break;
                }
                    
                return errorMessage;
            }
        }

        public ManageGamesViewModel()
        {
            _gameService = ServiceSingletons.GameService;
            _presetService = ServiceSingletons.PresetService;

            GamesViewModel = ViewModelSingletons.GamesViewModel;

            DeleteGameCommand = new ActionCommand(DeleteGame, CanDelete);
            AddGameCommand = new ActionCommand(AddGame, CanAdd);
            BrowseCommand = new ActionCommand(BrowseForGameFile, () => true);
        }
        
        private void AddGame()
        {
            try
            {
                _gameService.Add(new Game(Name, ExecutablePath));
            }
            catch (InvalidOperationException exception)
            {
                MessageBoxUtil.ShowError(exception.Message);
            }
        }

        private bool CanAdd()
        {
            return !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(ExecutablePath);
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
                ExecutablePath = openFileDialog.FileName;
            }
        }

        private void DeleteGame()
        {
            try
            {
                _gameService.Delete(_selectedGame);
                _presetService.DeleteByGame(_selectedGame);
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