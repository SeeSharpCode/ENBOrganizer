using ENBOrganizer.Domain;
using ENBOrganizer.Domain.Services;
using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Util;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Diagnostics;

namespace ENBOrganizer.App.ViewModels
{
    public class GamesViewModel : ViewModelBase, IPageViewModel
    {
        private readonly GameService _gameService;
        private readonly DialogService _dialogService;

        public string Name { get { return "Games"; } }
        public ICommand ShowAddGameDialogCommand { get; set; }
        public ICommand DeleteGameCommand { get; set; }
        public ICommand OpenGameFolderCommand { get; set; }

        public ObservableCollection<Game> Games { get; set; }

        public Game CurrentGame
        {
            get { return Properties.Settings.Default.CurrentGame; }
            set
            {
                Properties.Settings.Default.CurrentGame = value;
                Properties.Settings.Default.Save();

                RaisePropertyChanged(nameof(CurrentGame), true);
            }
        }

        public GamesViewModel(GameService gameService, PresetService presetService, DialogService dialogService)
        {
            _gameService = gameService;
            _gameService.ItemsChanged += _gameService_ItemsChanged;

            _dialogService = dialogService;

            // ShowAddGameDialogCommand = new RelayCommand(() => _dialogService.ShowDialog(Dialog.AddGame));
            DeleteGameCommand = new RelayCommand(() => _gameService.Delete(CurrentGame), () => CurrentGame != null);
            OpenGameFolderCommand = new RelayCommand(() => Process.Start(CurrentGame.DirectoryPath), () => CurrentGame != null);

            Games = _gameService.GetAll().ToObservableCollection();
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
            {
                Games.Remove(game);
                CurrentGame = Games.FirstOrDefault();
            }
        }
    }
}
