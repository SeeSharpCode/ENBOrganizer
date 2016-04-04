using ENBOrganizer.Domain;
using ENBOrganizer.Domain.Services;
using ENBOrganizer.Model.Entities;
using ENBOrganizer.Util;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Forms;
using System.Windows.Input;

namespace ENBOrganizer.App.ViewModels
{
    public class GamesViewModel : ObservableObject
    {
        private readonly GameService _gameService;

        public ObservableCollection<Game> Games { get; set; }
        public ICommand AddGameCommand { get; set; }
        public ICommand BrowseCommand { get; set; }

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

        private Game _selectedGame;

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

        public GamesViewModel()
        {
            _gameService = new GameService();
            _gameService.ItemsChanged += OnGamesChanged;

            Games = _gameService.GetAll().ToObservableCollection();

            AddGameCommand = new ActionCommand(AddGame, CanAdd);
            BrowseCommand = new ActionCommand(BrowseForGameFile, () => true);
        }

        private void OnGamesChanged(object sender, RepositoryChangedEventArgs eventArgs)
        {
            if (eventArgs.RepositoryActionType == RepositoryActionType.Added)
                Games.Add(eventArgs.Entity as Game);
            else
                Games.Remove(eventArgs.Entity as Game);
        }

        private bool CanAdd()
        {
            return !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(ExecutablePath);
        }

        private void AddGame()
        {
            try
            {
                _gameService.Add(new Game(Name, ExecutablePath));
            }
            catch (InvalidOperationException exception)
            {
                // TODO: MessageBoxUtil.ShowError(exception.Message);
            }
        }

        private void BrowseForGameFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "EXE Files (*.exe)|*.exe",
                Title = "Select the game's .exe file"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Name = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
                ExecutablePath = openFileDialog.FileName;
            }
        }
    }
}
