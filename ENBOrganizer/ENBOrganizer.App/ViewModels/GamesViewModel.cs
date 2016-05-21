using ENBOrganizer.App.Messages;
using ENBOrganizer.Domain;
using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Services;
using ENBOrganizer.Util;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;

namespace ENBOrganizer.App.ViewModels
{
    public class GamesViewModel : ViewModelBase, IPageViewModel
    {
        private readonly GameService _gameService;
        private readonly DialogService _dialogService;

        public string Name { get { return "Games"; } }
        public ICommand OpenAddGameDialogCommand { get; set; }
        public ICommand DeleteGameCommand { get; set; }
        public ICommand ViewFilesCommand { get; set; }
        public ICommand EditGameCommand { get; set; }
        public ObservableCollection<Game> Games { get; set; }

        public Game CurrentGame
        {
            get { return Properties.Settings.Default.CurrentGame; }
            set
            {
                Properties.Settings.Default.CurrentGame = value;
                RaisePropertyChanged(nameof(CurrentGame));
            }
        }
        
        private bool _isAddGameDialogOpen;

        public bool IsAddGameDialogOpen
        {
            get { return _isAddGameDialogOpen; }
            set
            {
                Set(nameof(IsAddGameDialogOpen), ref _isAddGameDialogOpen, value);

                if (!value)
                    PopulateGames();
            }
        }
        
        public GamesViewModel(GameService gameService, PresetService presetService, DialogService dialogService)
        {
            _gameService = gameService;
            _gameService.ItemsChanged += _gameService_ItemsChanged;

            _dialogService = dialogService;

            MessengerInstance.Register<DialogMessage>(this, OnDialogMessage);

            OpenAddGameDialogCommand = new RelayCommand(() => _dialogService.OpenDialog(DialogName.AddGame));
            EditGameCommand = new RelayCommand<Game>(EditGame);
            ViewFilesCommand = new RelayCommand<Game>(game => Process.Start(game.DirectoryPath));
            DeleteGameCommand = new RelayCommand<Game>(game => _gameService.Delete(game));

            PopulateGames();
        }

        private void PopulateGames()
        {
            if (Games == null)
                Games = new ObservableCollection<Game>();

            Games.Clear();
            Games.AddAll(_gameService.GetAll().ToObservableCollection());
        }

        private void EditGame(Game game)
        {
            _dialogService.OpenDialog(DialogName.AddGame);
            MessengerInstance.Send(game);
        }

        private void OnDialogMessage(DialogMessage dialogMessage)
        {
            if (dialogMessage.DialogName != DialogName.AddGame)
                return;

            IsAddGameDialogOpen = dialogMessage.DialogAction == DialogAction.Open;
        }

        private void _gameService_ItemsChanged(object sender, RepositoryChangedEventArgs repositoryChangedEventArgs)
        {
            Game game = repositoryChangedEventArgs.Entity as Game;

            if (repositoryChangedEventArgs.RepositoryActionType == RepositoryActionType.Added)
            {
                Games.Add(game);

                if (CurrentGame == null)
                    CurrentGame = game;
            }
            else
            {
                Games.Remove(game);

                if (CurrentGame == game)
                    CurrentGame = Games.FirstOrDefault();
            }
        }
    }
}
