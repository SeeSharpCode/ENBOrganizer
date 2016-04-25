using ENBOrganizer.Util.IO;
using System.Collections.Generic;
using System.IO;

namespace ENBOrganizer.Model.Entities
{
    public class PresetDirectory : IPresetItem
    {
        public string Name { get; set; }
        public string Path { get; set; }

        public List<IPresetItem> Items { get; set; }

        public PresetDirectory(string name, string path, List<IPresetItem> presetItems)
        {
            Name = name;
            Path = path;
            Items = presetItems;
        }

        public void Rename(string newName)
        {
            try
            {
                DirectoryInfo directory = new DirectoryInfo(Path);
                directory.Rename(newName);
            }
            catch (IOException)
            {
                throw;
            }
            
        }

        public void Delete()
        {
            Directory.Delete(Path, true);
        }
    }
}
