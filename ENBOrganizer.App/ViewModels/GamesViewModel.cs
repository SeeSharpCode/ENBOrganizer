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
        public ICommand DeleteGameCommand { get; set; }

        private bool _isAddGameFlyoutOpen;

        public bool IsAddGameFlyoutOpen
        {
            get { return _isAddGameFlyoutOpen; }
            set
            {
                _isAddGameFlyoutOpen = value;
                RaisePropertyChanged("IsAddGameFlyoutOpen");
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

        public Game CurrentGame
        {
            get { return _gameService.CurrentGame; }
            set { _gameService.CurrentGame = value; }
        }

        public GamesViewModel(GameService gameService)
        {
            _gameService = gameService;
            _gameService.ItemsChanged += OnGamesChanged;

            Games = _gameService.GetAll().ToObservableCollection();

            AddGameCommand = new ActionCommand(AddGame, () => true);
            BrowseCommand = new ActionCommand(BrowseForGameFile, () => true);
            DeleteGameCommand = new ActionCommand(DeleteGame, CanDelete);
        }

        private void DeleteGame()
        {
            try
            {
                _gameService.Delete(CurrentGame);
                // TODO: _presetService.DeleteByGame(_selectedGame);
            }
            catch (Exception exception)
            {
                //TODO: MessageBoxUtil.ShowError(exception.Message);
            }
        }

        private bool CanDelete()
        {
            return CurrentGame != null;
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
            IsAddGameFlyoutOpen = true;
            //try
            //{
            //    _gameService.Add(new Game(Name, ExecutablePath));
            //}
            //catch (InvalidOperationException exception)
            //{
            //    // TODO: MessageBoxUtil.ShowError(exception.Message);
            //}
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
