using System.ComponentModel;
using ENBOrganizer.Domain.Services;
using ENBOrganizer.Model.Entities;
using ENBOrganizer.Util;

namespace ENBOrganizer.UI.ViewModels
{
    public class ActiveGameViewModel : ObservableObject
    {
        private readonly GameService _gameService;

        public GamesViewModel GamesViewModel { get; set; }

        public Game ActiveGame
        {
            get { return _gameService.ActiveGame; } 
            set { _gameService.ActiveGame = value; }
        }

        public ActiveGameViewModel()
            : this(ViewModelSingletons.GamesViewModel, ServiceSingletons.GameService)
        { }

        private ActiveGameViewModel(GamesViewModel gamesViewModel, GameService gameService)
        {
            GamesViewModel = gamesViewModel;

            _gameService = gameService;
            _gameService.PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName.Equals("ActiveGame"))
                RaisePropertyChanged("ActiveGame");
        }
    }
}
