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
            get { return _gameService.CurrentGame; } 
            set { _gameService.CurrentGame = value; }
        }

        public ActiveGameViewModel()
        {
            _gameService = ServiceSingletons.GameService;

            _gameService.PropertyChanged += OnPropertyChanged;

            GamesViewModel = ViewModelSingletons.GamesViewModel;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName.Equals("ActiveGame"))
                RaisePropertyChanged("ActiveGame");
        }
    }
}
