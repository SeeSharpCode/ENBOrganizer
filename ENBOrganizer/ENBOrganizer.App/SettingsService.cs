using ENBOrganizer.App.Properties;
using ENBOrganizer.Domain.Entities;
using GalaSoft.MvvmLight;

namespace ENBOrganizer.App
{
    public class SettingsService : ObservableObject
    {
        public bool UpdateSettings
        {
            get { return Settings.Default.UpdateSettings; }
            set
            {
                Settings.Default.UpdateSettings = value;
                Settings.Default.Save();
            }
        }

        public bool FirstUse
        {
            get { return Settings.Default.FirstUse; }
            set
            {
                Settings.Default.FirstUse = value;
                Settings.Default.Save();
            }
        }

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

        public void UpgradeSettings()
        {
            if (!UpdateSettings)
                return;

            Settings.Default.Upgrade();
            UpdateSettings = false;
        }
    }
}
