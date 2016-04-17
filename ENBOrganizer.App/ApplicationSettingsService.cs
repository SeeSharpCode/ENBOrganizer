using ENBOrganizer.Model.Entities;
using GalaSoft.MvvmLight;

namespace ENBOrganizer.App
{
    public class ApplicationSettingsService : ObservableObject
    {
        public Game CurrentGame
        {
            get { return Properties.Settings.Default.CurrentGame; }
            set
            {
                Properties.Settings.Default.CurrentGame = value;
                Properties.Settings.Default.Save();

                RaisePropertyChanged("CurrentGame"); 
            }
        }
    }
}
