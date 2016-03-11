namespace ENBOrganizer.UI.ViewModels
{
    public static class ViewModelSingletons
    {
        private static GamesViewModel _gamesViewModel;

        public static GamesViewModel GamesViewModel
        {
            get { return _gamesViewModel ?? (_gamesViewModel = new GamesViewModel()); }
        }

    }
}
