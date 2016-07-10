using ENBOrganizer.App.Properties;
using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Services;
using GalaSoft.MvvmLight;
using System.Linq;

namespace ENBOrganizer.App
{
    public class SettingsService : ObservableObject
    {
        private readonly GameService _gameService;
        private readonly MasterListService _masterListService;

        public Game CurrentGame
        {
            get { return Settings.Default.CurrentGame; }
            set
            {
                Settings.Default.CurrentGame = value;
                Settings.Default.Save();

                RaisePropertyChanged(nameof(CurrentGame));
            }
        }

        public SettingsService(GameService gameService, MasterListService masterListService)
        {
            _gameService = gameService;
            _masterListService = masterListService;
        }

        public void InitializeSettings()
        {
            if (Settings.Default.UpdateSettings)
                UpgradeSettings();

            if (Settings.Default.FirstUse)
                SetupGamesOnFirstUse();

            if (!_masterListService.Items.Any())
                _masterListService.AddDefaultItems();
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
