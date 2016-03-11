using System.Configuration;
using System.IO;
using System.Xml.Serialization;
using ENBOrganizer.Util;

namespace ENBOrganizer.Model.Entities
{
    [SettingsSerializeAs(SettingsSerializeAs.Xml)]
    public class Game : IEntity
    {
        public string Name { get; set; }
        public string ExecutablePath { get; set; } 

        [XmlIgnore]
        public DirectoryInfo PresetsDirectory
        {
            get
            {
                string path = Path.Combine(Paths.Games, Name);
                return new DirectoryInfo(path); 
            }
        } 

        public Game() { } // Required for serialization.

        public Game(string name, string executablePath)
        {
            Name = name;
            ExecutablePath = executablePath;
        }

        public override bool Equals(object other)
        {
            Game game = other as Game;

            if (game == null)
                return false;

            return Name.EqualsIgnoreCase(game.Name);
        }
    }
}
