﻿using ENBOrganizer.App.Messages;
using ENBOrganizer.Domain;
using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Services;
using GalaSoft.MvvmLight.CommandWpf;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;

namespace ENBOrganizer.App.ViewModels
{
    public class GamesViewModel : PageViewModelBase<Game>
    {
        public ICommand OpenDirectoryCommand { get; set; }
        public ICommand EditGameCommand { get; set; }

        public new Game CurrentGame
        {
            get { return Properties.Settings.Default.CurrentGame; }
            set
            {
                Properties.Settings.Default.CurrentGame = value;
                Properties.Settings.Default.Save();

                RaisePropertyChanged(nameof(CurrentGame));
            }
        }
        
        public GamesViewModel(GameService gameService, DialogService dialogService)
            : base(gameService, dialogService, DialogName.AddGame, "Games")
        {
            EditGameCommand = new RelayCommand<Game>(EditGame);
            OpenDirectoryCommand = new RelayCommand<Game>(game => Process.Start(game.DirectoryPath));
        }

        protected new bool CanAdd()
        {
            return true;
        }

        private void EditGame(Game game)
        {
            _dialogService.ShowDialog(DialogName.AddGame);
            MessengerInstance.Send(game);
        }

        private new void _dataService_ItemsChanged(object sender, RepositoryChangedEventArgs repositoryChangedEventArgs)
        {
            Game game = repositoryChangedEventArgs.Entity as Game;

            if (repositoryChangedEventArgs.RepositoryActionType == RepositoryActionType.Added)
            {
                Models.Add(game);

                if (CurrentGame == null)
                    CurrentGame = game;
            }
            else
            {
                Models.Remove(game);

                if (CurrentGame == game)
                    CurrentGame = Models.FirstOrDefault();
            }
        }
    }
}
