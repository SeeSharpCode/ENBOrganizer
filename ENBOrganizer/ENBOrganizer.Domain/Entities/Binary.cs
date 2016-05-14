using System.IO;
using System.Xml.Serialization;

namespace ENBOrganizer.Domain.Entities
{
    public class Binary : IEntity
    {
        public string Name { get; set; }
        public Game Game { get; set; }

        [XmlIgnore]
        public DirectoryInfo Directory
        {
            get
            {
                string path = Path.Combine(Game.PresetsDirectory.FullName, Name);
                return new DirectoryInfo(path);
            }
        }
    }
}
