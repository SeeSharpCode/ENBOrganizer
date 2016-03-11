using System.Collections.ObjectModel;
using ENBOrganizer.Domain;
using ENBOrganizer.Domain.Services;
using ENBOrganizer.Model.Entities;
using ENBOrganizer.Util;

namespace ENBOrganizer.UI.ViewModels
{
    public class GamesViewModel : ObservableObject
    {
        private readonly GameService _gameService;

        public ObservableCollection<Game> Games { get; set; }

        public GamesViewModel()
            : this(ServiceSingletons.GameService)
        { }

        private GamesViewModel(GameService gameService)
        {
            _gameService = gameService;
            _gameService.GamesChanged += OnGamesChanged;

            Games = new ObservableCollection<Game>(_gameService.GetAll());
        }

        private void OnGamesChanged(object sender, RepositoryChangedEventArgs repositoryChangedEventArgs)
        {
            Game game = repositoryChangedEventArgs.Entity as Game;

            if (repositoryChangedEventArgs.RepositoryActionType == RepositoryActionType.Added)
                Games.Add(game);
            else
                Games.Remove(game);
        }
    }
}
