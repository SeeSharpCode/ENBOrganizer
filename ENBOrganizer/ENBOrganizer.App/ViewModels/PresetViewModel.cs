using ENBOrganizer.Domain.Entities;
using GalaSoft.MvvmLight;

namespace ENBOrganizer.App.ViewModels
{
    public class PresetViewModel : ViewModelBase
    {
        public Preset Preset { get; set; }

        public string Name
        {
            get { return Preset.Name; }
            set
            {
                Preset.Name = value;
                RaisePropertyChanged("Name");
            }
        }

        public string ImagePath
        {
            get { return Preset.ImagePath; }
            set
            {
                Preset.ImagePath = value;
                RaisePropertyChanged("ImagePath");
            }
        }

        public bool IsEnabled
        {
            get { return Preset.IsEnabled; }
            set
            {
                Preset.IsEnabled = value;
                RaisePropertyChanged("IsEnabled");
            }
        }

        public PresetViewModel(Preset preset)
        {
            Preset = preset;
        }
    }
}
