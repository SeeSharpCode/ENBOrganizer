using System.IO;

namespace ENBOrganizer.Domain.Entities
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class Binary : FileSystemEntity
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        public override DirectoryInfo Directory
        {
            get
            {
                string path = Path.Combine(Game.BinariesDirectory.FullName, Name);
                return new DirectoryInfo(path);
            }
        }

        public Binary() { } // Required for XML serialization.

        public Binary(string name, Game game) : base(name, game) { }
    }
}
