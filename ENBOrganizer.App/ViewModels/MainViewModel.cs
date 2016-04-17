using GalaSoft.MvvmLight;

namespace ENBOrganizer.App.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public GamesViewModel GamesViewModel { get; set; }

        public MainViewModel(GamesViewModel gamesViewModel)
        {
            GamesViewModel = gamesViewModel;
        }
    }
}
