using ENBOrganizer.Util;
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
                string path = Path.Combine(Game.BinariesDirectory.FullName, Name);
                return new DirectoryInfo(path);
            }
        }

        public override bool Equals(object other)
        {
            Binary binary = other as Binary;

            return binary != null ? Name.EqualsIgnoreCase(binary.Name) && Game.Equals(binary.Game) : false;
        }
    }
}
