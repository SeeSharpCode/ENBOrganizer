using ENBOrganizer.App.Messages;
using ENBOrganizer.App.Properties;
using ENBOrganizer.Domain;
using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Services;
using GalaSoft.MvvmLight.CommandWpf;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;

namespace ENBOrganizer.App.ViewModels.Games
{
    public class GamesViewModel : PageViewModelBase<Game>
    {
        protected override DialogName DialogName { get { return DialogName.GameDetail; } }

        public ICommand ViewFilesCommand { get; set; }
        public ICommand EditGameCommand { get; set; }
        
        public GamesViewModel(GameService gameService)
            : base(gameService)
        {
            EditGameCommand = new RelayCommand<Game>(EditGame);
            ViewFilesCommand = new RelayCommand<Game>(game => Process.Start(game.ExecutableDirectory.FullName), CanViewFiles);
        }

        private bool CanViewFiles(Game game)
        {
            if (game == null)
                return false;

            return game.ExecutableExists;
        }

        protected override bool CanAdd()
        {
            return true;
        }

        private void EditGame(Game game)
        {
            _dialogService.ShowDialog(DialogName.GameDetail);
            MessengerInstance.Send(game);
        }

        protected override void _dataService_ItemsChanged(object sender, RepositoryChangedEventArgs repositoryChangedEventArgs)
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
