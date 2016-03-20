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
        {
            _gameService = ServiceSingletons.GameService;
            _gameService.ItemsChanged += OnGamesChanged;

            Games = _gameService.GetAll().ToObservableCollection();
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
