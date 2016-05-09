using ENBOrganizer.Util;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;

namespace ENBOrganizer.Domain.Entities
{
    // TODO: remove INotifyPropertyChanged from this model class
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class Preset : IEntity, INotifyPropertyChanged
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        public string Name { get; set; }
        public Game Game { get; set; }

        private string _imagePath;
        public string ImagePath
        {
            get { return _imagePath; }
            set
            {
                _imagePath = value;
                RaisePropertyChanged("ImagePath");
            }
        }

        private bool _isEnabled;

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;
                RaisePropertyChanged("IsEnabled"); }
        }

        [XmlIgnore]
        public DirectoryInfo Directory
        {
            get
            {
                string path = Path.Combine(Game.PresetsDirectory.FullName, Name);
                return new DirectoryInfo(path); 
             }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Preset() { } // Required for serialization.

        public Preset(string name, Game game)
        {
            Name = name;
            Game = game;
        }
        
        public override bool Equals(object other)
        {
            Preset preset = other as Preset;

            if (preset == null)
                return false;

            return Name.EqualsIgnoreCase(preset.Name) && Game.Equals(preset.Game);
        }

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
