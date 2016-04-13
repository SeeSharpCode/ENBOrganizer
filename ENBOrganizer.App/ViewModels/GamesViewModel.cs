using ENBOrganizer.Domain;
using ENBOrganizer.Domain.Services;
using ENBOrganizer.Model.Entities;
using ENBOrganizer.Util;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace ENBOrganizer.App.ViewModels
{
    public class GamesViewModel : ObservableObject
    {
        private readonly GameService _gameService;

        public ObservableCollection<Game> Games { get; set; }
        public ICommand ShowAddGameDialogCommand { get; set; }
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
        
        public Game CurrentGame
        {
            get { return _gameService.CurrentGame; }
            set { _gameService.CurrentGame = value; }
        }

        public GamesViewModel(GameService gameService)
        {
            _gameService = gameService;
            _gameService.ItemsChanged += OnGamesChanged;
            _gameService.PropertyChanged += _gameService_PropertyChanged;

            Games = _gameService.GetAll().ToObservableCollection();
            
            ShowAddGameDialogCommand = new ActionCommand(() => IsAddGameFlyoutOpen = true, () => true);
            DeleteGameCommand = new ActionCommand(DeleteGame, CanDelete);
        }

        private void _gameService_PropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == "CurrentGame")
                RaisePropertyChanged("CurrentGame");
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
    }
}
