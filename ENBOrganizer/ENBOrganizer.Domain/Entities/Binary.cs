using System.ComponentModel.DataAnnotations.Schema;
using SystemIO = System.IO;

namespace ENBOrganizer.Domain.Entities
{
    public class Binary : FileSystemEntity
    {
        public long BinaryId { get; set; }

        [NotMapped]
        public override SystemIO.DirectoryInfo Directory
        {
            get
            {
                string path = SystemIO.Path.Combine(Game.BinariesDirectory.FullName, Name);
                return new SystemIO.DirectoryInfo(path);
            }
        }
        
        public Binary() { }

        public Binary(string name, Game game) : base(name, game) { }
    }
}
