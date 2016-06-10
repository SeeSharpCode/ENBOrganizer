using ENBOrganizer.App.Properties;
using ENBOrganizer.Domain.Services;

namespace ENBOrganizer.App
{
    public class SettingsService
    {
        private readonly GameService _gameService;

        public SettingsService(GameService gameService)
        {
            _gameService = gameService;
        }

        public void InitializeSettings()
        {
            if (Settings.Default.UpdateSettings)
                UpgradeSettings();

            if (Settings.Default.FirstUse)
                SetupGamesOnFirstUse();            
        }

        private void UpgradeSettings()
        {
            Settings.Default.Upgrade();
            Settings.Default.UpdateSettings = false;
            Settings.Default.Save();
        }

        private void SetupGamesOnFirstUse()
        {
            _gameService.AddGamesFromRegistry();

            Settings.Default.FirstUse = false;
            Settings.Default.Save();
        }
    }
}
