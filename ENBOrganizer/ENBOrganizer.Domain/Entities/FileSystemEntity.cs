using ENBOrganizer.Util;
using ENBOrganizer.Util.IO;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace ENBOrganizer.Domain.Entities
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public abstract class FileSystemEntity : EntityBase
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
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

        public virtual void Enable()
        {
            Directory.CopyTo(Game.ExecutableDirectory.FullName);
        }

        public virtual void Disable()
        {
            foreach (FileSystemInfo fileSystemInfo in Directory.GetFileSystemInfos())
            {
                if (DirectoryNames.EssentialNames.Any(name => name.EqualsIgnoreCase(fileSystemInfo.Name)) || !fileSystemInfo.Exists)
                    continue;

                string installedPath = Path.Combine(Game.ExecutableDirectory.FullName, fileSystemInfo.Name);

                if (fileSystemInfo is DirectoryInfo)
                    (fileSystemInfo as DirectoryInfo).DeleteRecursive();
                else if (File.Exists(installedPath))
                    File.Delete(installedPath);
            }
        }

        public void ChangeState()
        {
            if (IsEnabled)
                Disable();
            else
                Enable();

            IsEnabled = !IsEnabled;
        }

        public override bool Equals(object other)
        {
            FileSystemEntity entity = other as FileSystemEntity;

            if (entity == null)
                return false;

            return Name.EqualsIgnoreCase(entity.Name) && Game.Equals(entity.Game);
        }
    }
}
