using ENBOrganizer.Util;
using System.IO;
using System.Xml.Serialization;

namespace ENBOrganizer.Domain.Entities
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class Preset : EntityBase
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        public Game Game { get; set; }
        public Binary Binary { get; set; }
        public string ImagePath { get; set; }
        public bool IsEnabled { get; set; }

        [XmlIgnore]
        public DirectoryInfo Directory
        {
            get
            {
                string path = Path.Combine(Game.PresetsDirectory.FullName, Name);
                return new DirectoryInfo(path); 
             }
        }

        public Preset() { } // Required for serialization.

        public Preset(string name, Game game)
        {
            Name = name;
            Game = game;
        }
        
        public override bool Equals(object other)
        {
            Preset preset = other as Preset;

            return preset != null ? Name.EqualsIgnoreCase(preset.Name) && Game.Equals(preset.Game) : false;
        }
    }
}
