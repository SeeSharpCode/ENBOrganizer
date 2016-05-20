using ENBOrganizer.Util;
using System.IO;
using System.Xml.Serialization;

namespace ENBOrganizer.Domain.Entities
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class Binary : EntityBase
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
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

        public Binary() { } // Required for serialization.

        public Binary(string name, Game game)
        {
            Name = name;
            Game = game;
        }

        public override bool Equals(object other)
        {
            Binary binary = other as Binary;

            return binary != null ? Name.EqualsIgnoreCase(binary.Name) && Game.Equals(binary.Game) : false;
        }
    }
}
