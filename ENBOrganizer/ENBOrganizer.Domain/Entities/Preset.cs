using System.IO;

namespace ENBOrganizer.Domain.Entities
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class Preset : FileSystemEntity
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        public Binary Binary { get; set; }
        public string ImagePath { get; set; }
        public string Description { get; set; }

        public override DirectoryInfo Directory
        {
            get
            {
                string path = Path.Combine(Game.PresetsDirectory.FullName, Name);
                return new DirectoryInfo(path); 
             }
        }

        public Preset() { } // Required for XML serialization.

        public Preset(string name, Game game) : base(name, game) { }
    }
}
