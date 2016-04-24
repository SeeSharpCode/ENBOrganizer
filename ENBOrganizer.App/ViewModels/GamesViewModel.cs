using ENBOrganizer.Domain;
using ENBOrganizer.Domain.Services;
using ENBOrganizer.Model.Entities;
using ENBOrganizer.Util;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ENBOrganizer.App.ViewModels
{
    public class GamesViewModel : ViewModelBase
    {
        private readonly GameService _gameService;
        
        public ICommand ShowAddGameDialogCommand { get; set; }
        public ICommand DeleteGameCommand { get; set; }

        private ObservableCollection<Game> _games;

        public ObservableCollection<Game> Games
        {
            get { return _games; }
            set
            {
                _games = value;
                RaisePropertyChanged("Games");
            }
        }

        public Game CurrentGame
        {
            get { return Properties.Settings.Default.CurrentGame; }
            set
            {
                Properties.Settings.Default.CurrentGame = value;
                Properties.Settings.Default.Save();

                RaisePropertyChanged("CurrentGame", null, value, true);
            }
        }

        public GamesViewModel(GameService gameService)
        {
            _gameService = gameService;
            _gameService.ItemsChanged += _gameService_ItemsChanged;

            ShowAddGameDialogCommand = new RelayCommand(() => DialogService.ShowAddGameDialog());
            DeleteGameCommand = new RelayCommand(DeleteGame, CanDelete);

            Games = _gameService.GetAll().ToObservableCollection();
            MessengerInstance.Send(new PropertyChangedMessage<Game>(null, CurrentGame, "CurrentGame"));
        }

        private void _gameService_ItemsChanged(object sender, RepositoryChangedEventArgs repositoryChangedEventArgs)
        {
            Game game = repositoryChangedEventArgs.Entity as Game;

            if (repositoryChangedEventArgs.RepositoryActionType == RepositoryActionType.Added)
            {
                Games.Add(game);
                CurrentGame = game;
            }                
            else
                Games.Remove(game);
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
    }
}
