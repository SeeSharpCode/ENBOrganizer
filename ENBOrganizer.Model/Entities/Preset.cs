using ENBOrganizer.Util;
using System.IO;
using System.Xml.Serialization;

namespace ENBOrganizer.Model.Entities
{
    // TODO: remove entities folder from Model project
    public class Preset : IEntity
    {
        public string Name { get; set; }
        public Game Game { get; set; }
        public string ImagePath { get; set; }

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

            if (preset == null)
                return false;

            return Name.EqualsIgnoreCase(preset.Name) && Game.Equals(preset.Game);
        }
    }
}
