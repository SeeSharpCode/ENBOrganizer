namespace ENBOrganizer.App.ViewModels
{
    public class MainViewModel
    {
        public GamesViewModel GamesViewModel { get; set; }

        public MainViewModel(GamesViewModel gamesViewModel)
        {
            GamesViewModel = gamesViewModel;
        }
    }
}
