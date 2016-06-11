using ENBOrganizer.Util;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.IO;
using System.Xml.Serialization;

namespace ENBOrganizer.Domain.Entities
{
    [SettingsSerializeAs(SettingsSerializeAs.Xml)]
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class Game : EntityBase
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        public long GameId { get; set; }

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

        [NotMapped]
        public bool ExecutableExists { get { return File.Exists(ExecutablePath); } }
        
        [NotMapped]
        public DirectoryInfo PresetsDirectory
        {
            get
            {
                string path = Path.Combine(DirectoryNames.Games, Name, DirectoryNames.Presets);
                return new DirectoryInfo(path); 
            }
        }

        [NotMapped]
        public DirectoryInfo BinariesDirectory
        {
            get
            {
                string path = Path.Combine(DirectoryNames.Games, Name, DirectoryNames.Binaries);
                return new DirectoryInfo(path);
            }
        }

        [NotMapped]
        public DirectoryInfo Directory
        {
            get { return new DirectoryInfo(Path.GetDirectoryName(ExecutablePath)); }
        }

        public Game(string name, string executablePath)
            : base(name)
        {
            ExecutablePath = executablePath;
        }

        public override bool Equals(object other)
        {
            Game game = other as Game;

            if (game == null)
                return false;

            return Name.EqualsIgnoreCase(game.Name) || Directory.FullName.Equals(game.Directory.FullName);
        }
    }
}
