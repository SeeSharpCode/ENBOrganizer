using ENBOrganizer.App.Messages;
using ENBOrganizer.Domain;
using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Services;
using ENBOrganizer.Util;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;

namespace ENBOrganizer.App.ViewModels
{
    public class GamesViewModel : PageViewModelBase<Game>
    {
        public override string Name { get { return "Games"; } }
        public ICommand DeleteGameCommand { get; set; }
        public ICommand OpenDirectoryCommand { get; set; }
        public ICommand EditGameCommand { get; set; }

        public Game CurrentGame
        {
            get { return Properties.Settings.Default.CurrentGame; }
            set
            {
                Properties.Settings.Default.CurrentGame = value;
                Properties.Settings.Default.Save();

                RaisePropertyChanged(nameof(CurrentGame));
            }
        }
        
        private bool _isAddGameDialogOpen;

        public bool IsAddGameDialogOpen
        {
            get { return _isAddGameDialogOpen; }
            set { Set(nameof(IsAddGameDialogOpen), ref _isAddGameDialogOpen, value); }
        }
        
        public GamesViewModel()
            : base(SimpleIoc.Default.GetInstance<GameService>(), SimpleIoc.Default.GetInstance<DialogService>(), DialogName.AddGame)
        {
            EditGameCommand = new RelayCommand<Game>(EditGame);
            OpenDirectoryCommand = new RelayCommand<Game>(game => Process.Start(game.DirectoryPath));
        }

        protected override void PopulateModels()
        {
            if (Models == null)
                Models = new ObservableCollection<Game>();

            Models.Clear();
            Models.AddAll(_dataService.GetAll().ToObservableCollection());
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

        protected override bool CanSave()
        {
            return true;
        }
    }
}
