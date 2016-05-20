using System.Configuration;
using System.IO;
using System.Xml.Serialization;
using ENBOrganizer.Util;
using System.Collections.Generic;

namespace ENBOrganizer.Domain.Entities
{
    [SettingsSerializeAs(SettingsSerializeAs.Xml)]
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class Game : EntityBase
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        private string _executablePath;

        public string ExecutablePath
        {
            get { return _executablePath; }
            set
            {
                _executablePath = value;
                RaisePropertyChanged(nameof(ExecutablePath));
            }
        }

        [XmlIgnore]
        public List<Preset> Presets { get; set; }

        [XmlIgnore]
        public DirectoryInfo PresetsDirectory
        {
            get
            {
                string path = Path.Combine(DirectoryNames.Games, Name, DirectoryNames.Presets);
                return new DirectoryInfo(path); 
            }
        }

        [XmlIgnore]
        public DirectoryInfo BinariesDirectory
        {
            get
            {
                string path = Path.Combine(DirectoryNames.Games, Name, DirectoryNames.Binaries);
                return new DirectoryInfo(path);
            }
        }

        [XmlIgnore]
        public string DirectoryPath
        {
            get { return Path.GetDirectoryName(ExecutablePath); }
        }

        public Game() { } // Required for serialization.

        public Game(string name, string executablePath)
            : base(name)
        {
            ExecutablePath = executablePath;
        }

        public override bool Equals(object other)
        {
            Game game = other as Game;

            return game != null ? ID.EqualsIgnoreCase(game.ID) : false;
        }
    }
}
