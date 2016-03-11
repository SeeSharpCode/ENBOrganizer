namespace ENBOrganizer.Domain.Services
{
    public static class ServiceSingletons
    {
        private static GameService _gameService;

        public static GameService GameService
        {
            get { return _gameService ?? (_gameService = new GameService()); }
        }

        private static PresetService _presetService;

        public static PresetService PresetService
        {
            get { return _presetService ?? (_presetService = new PresetService()); }
        }
    }
}