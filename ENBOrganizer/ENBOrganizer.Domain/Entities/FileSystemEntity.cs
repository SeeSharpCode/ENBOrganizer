using System.IO;
using System.Xml.Serialization;

namespace ENBOrganizer.Domain.Entities
{
    public abstract class FileSystemEntity : EntityBase
    {
        public Game Game { get; set; }
        public bool IsEnabled { get; set; }

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
