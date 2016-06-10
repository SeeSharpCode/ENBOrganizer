using SystemIO = System.IO;

namespace ENBOrganizer.Domain.Entities
{
    public class Binary : FileSystemEntity
    {
        public override SystemIO.DirectoryInfo Directory
        {
            get
            {
                string path = SystemIO.Path.Combine(Game.BinariesDirectory.FullName, Name);
                return new SystemIO.DirectoryInfo(path);
            }
        }

        public Binary() { } // Required for XML serialization.

        public Binary(string name, Game game) : base(name, game) { }
    }
}
