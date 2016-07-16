using ENBOrganizer.App.Properties;
using ENBOrganizer.Domain.Entities;
using GalaSoft.MvvmLight;
using System.Windows;

namespace ENBOrganizer.App
{
    public class SettingsService : ObservableObject
    {
        private double _windowTop;

        public double WindowTop
        {
            get { return _windowTop; }
            set { _windowTop = value; }
        }

        private double _windowLeft;

        public double WindowLeft
        {
            get { return _windowLeft; }
            set { _windowLeft = value; }
        }

        private double _windowHeight;

        public double WindowHeight
        {
            get { return _windowHeight; }
            set { _windowHeight = value; }
        }

        private double _windowWidth;

        public double WindowWidth
        {
            get { return _windowWidth; }
            set { _windowWidth = value; }
        }

        private WindowState _windowState;

        public WindowState WindowState
        {
            get { return _windowState; }
            set { _windowState = value; }
        }

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

        public void LoadWindowPosition(Window window)
        {
            window.Top = Settings.Default.WindowTop;
            window.Left = Settings.Default.WindowLeft;
            window.Height = Settings.Default.WindowHeight;
            window.Width = Settings.Default.WindowWidth;
            window.WindowState = Settings.Default.WindowState;
        }

        public void SaveWindowPosition(Window window)
        {
            if (window.WindowState == WindowState.Minimized)
                return;

            Settings.Default.WindowTop = window.Top;
            Settings.Default.WindowLeft = window.Left;
            Settings.Default.WindowHeight = window.Height;
            Settings.Default.WindowWidth = window.Width;
            Settings.Default.WindowState = window.WindowState;

            Settings.Default.Save();
        }
    }
}
