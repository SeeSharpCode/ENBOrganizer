using ENBOrganizer.Util;
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
        public FileInfo GlobalENBLocalFile { get { return new FileInfo(Path.Combine(Directory.FullName, FileSystemNames.GlobalENBLocal)); } }

        [XmlIgnore]
        public FileInfo EnabledGlobalENBLocalFile { get { return new FileInfo(Path.Combine(ExecutableDirectory.FullName, FileSystemNames.GlobalENBLocal)); } }

        [XmlIgnore]
        public bool ExecutableExists { get { return File.Exists(ExecutablePath); } }
        
        [XmlIgnore]
        public DirectoryInfo PresetsDirectory
        {
            get
            {
                string path = Path.Combine(FileSystemNames.Games, Name, FileSystemNames.Presets);
                return new DirectoryInfo(path); 
            }
        }

        [XmlIgnore]
        public DirectoryInfo Directory
        {
            get
            {
                string path = Path.Combine(FileSystemNames.Games, Name);
                return new DirectoryInfo(path);
            }
        }

        [XmlIgnore]
        public DirectoryInfo BinariesDirectory
        {
            get
            {
                string path = Path.Combine(FileSystemNames.Games, Name, FileSystemNames.Binaries);
                return new DirectoryInfo(path);
            }
        }

        [XmlIgnore]
        public DirectoryInfo ExecutableDirectory
        {
            get { return new DirectoryInfo(Path.GetDirectoryName(ExecutablePath)); }
        }

        public Game() { } // Required for XML serialization.

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

            return Name.EqualsIgnoreCase(game.Name) && ExecutableDirectory.FullName.Equals(game.ExecutableDirectory.FullName);
        }

        public void EnableGlobalENBLocal()
        {
            if (!GlobalENBLocalFile.Exists)
                return;

            GlobalENBLocalFile.CopyTo(Path.Combine(ExecutableDirectory.FullName, GlobalENBLocalFile.Name), true);
        }

        public void DisableGlobalENBLocal()
        {
            if (!EnabledGlobalENBLocalFile.Exists)
                return;

            EnabledGlobalENBLocalFile.Delete();
        }
    }
}
