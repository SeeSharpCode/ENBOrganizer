using ENBOrganizer.App.Messages;
using ENBOrganizer.App.Properties;
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
        
        public new Game CurrentGame
        {
            get { return Settings.Default.CurrentGame; }
            set
            {
                Settings.Default.CurrentGame = value;
                Settings.Default.Save();

                RaisePropertyChanged(nameof(CurrentGame));
            }
        }
        
        public GamesViewModel(GameService gameService)
            : base(gameService)
        {
            EditGameCommand = new RelayCommand<Game>(EditGame);
            ViewFilesCommand = new RelayCommand<Game>(game => Process.Start(game.Directory.FullName), CanViewFiles);

            if (CurrentGame == null && Models.Any())
                CurrentGame = Models.First();
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
    }
}
