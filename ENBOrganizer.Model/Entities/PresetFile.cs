using System.IO;

namespace ENBOrganizer.Model.Entities
{
    public class PresetFile : PresetItem
    {
        public string Name { get; set; }
        public string Path { get; set; }

        public PresetFile(string name, string path)
        {
            Name = name;
            Path = path;
        }

        public void Delete()
        {
            File.Delete(Path);
        }
    }
}