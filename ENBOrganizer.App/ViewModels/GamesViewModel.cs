using ENBOrganizer.Domain;
using ENBOrganizer.Domain.Services;
using ENBOrganizer.Model.Entities;
using ENBOrganizer.Util;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ENBOrganizer.App.ViewModels
{
    public class GamesViewModel : ViewModelBase
    {
        private readonly GameService _gameService;
        private readonly ApplicationSettingsService _applicationSettingsService;

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

        private Game _currentGame;
        
        public Game CurrentGame
        {
            get { return _currentGame; }
            set
            {
                _currentGame = value;

                if (_applicationSettingsService.CurrentGame != value)
                    _applicationSettingsService.CurrentGame = value;
                
                RaisePropertyChanged("CurrentGame", null, _currentGame, true);
            }
        }

        public GamesViewModel(GameService gameService, ApplicationSettingsService applicationSettingsService)
        {
            _gameService = gameService;
            _gameService.ItemsChanged += _gameService_ItemsChanged; ;

            _applicationSettingsService = applicationSettingsService;

            Games = _gameService.GetAll().ToObservableCollection();
            CurrentGame = _applicationSettingsService.CurrentGame;
            
            ShowAddGameDialogCommand = new RelayCommand(() => IsAddGameFlyoutOpen = true, () => true);
            DeleteGameCommand = new RelayCommand(DeleteGame, CanDelete);
        }

        private void _gameService_ItemsChanged(object sender, RepositoryChangedEventArgs repositoryChangedEventArgs)
        {
            if (repositoryChangedEventArgs.RepositoryActionType == RepositoryActionType.Added)
                Games.Add(repositoryChangedEventArgs.Entity as Game);
            else
                Games.Remove(repositoryChangedEventArgs.Entity as Game);
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
