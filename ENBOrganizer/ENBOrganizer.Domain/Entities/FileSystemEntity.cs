using System.IO;
using System.Xml.Serialization;

namespace ENBOrganizer.Domain.Entities
{
    public abstract class FileSystemEntity : EntityBase
    {
        public Game Game { get; set; }

        private bool _isEnabled;

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;
                RaisePropertyChanged(nameof(IsEnabled));
            }
        }

        [XmlIgnore]
        public abstract DirectoryInfo Directory { get; }

        public FileSystemEntity() { } // Required for XML serialization.

        public FileSystemEntity(string name, Game game)
            : base(name)
        {
            Game = game;
        }
    }
}
