using ENBOrganizer.Util;
using ENBOrganizer.Util.IO;
using System.ComponentModel.DataAnnotations.Schema;
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
        
        [NotMapped]
        public abstract DirectoryInfo Directory { get; }
        
        public FileSystemEntity(string name, Game game)
            : base(name)
        {
            Game = game;
        }

        public virtual void Enable()
        {
            Directory.CopyTo(Game.Directory.FullName);
        }

        public virtual void Disable()
        {
            foreach (FileSystemInfo fileSystemInfo in Directory.GetFileSystemInfos())
            {
                if (DirectoryNames.EssentialNames.Any(name => name.EqualsIgnoreCase(fileSystemInfo.Name)))
                    continue;

                string installedPath = Path.Combine(Game.Directory.FullName, fileSystemInfo.Name);

                if (fileSystemInfo is DirectoryInfo && System.IO.Directory.Exists(installedPath) && fileSystemInfo.Name != DirectoryNames.Data)
                    System.IO.Directory.Delete(installedPath, true);
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
