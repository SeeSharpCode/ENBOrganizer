using ENBOrganizer.Util;
using ENBOrganizer.Util.IO;
using System;
using System.IO;
using System.Linq;
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

        public virtual void Enable()
        {
            Directory.CopyTo(Game.DirectoryPath);
        }

        public virtual void Disable()
        {
            foreach (FileSystemInfo fileSystemInfo in Directory.GetFileSystemInfos())
            {
                if (DirectoryNames.EssentialNames.Any(name => name.EqualsIgnoreCase(fileSystemInfo.Name)))
                    continue;

                string installedPath = Path.Combine(Game.DirectoryPath, fileSystemInfo.Name);

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
    }
}
