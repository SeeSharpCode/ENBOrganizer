using ENBOrganizer.Util.IO;
using System;
using System.Collections.Generic;
using System.IO;

namespace ENBOrganizer.Model.Entities
{
    public class PresetDirectory : PresetItem
    {
        public string Name { get; set; }
        public string Path { get; set; }

        public List<PresetItem> Items { get; set; }

        public PresetDirectory(string name, string path, List<PresetItem> presetItems)
        {
            Name = name;
            Path = path;
            Items = presetItems;
        }

        public void Rename(string newName)
        {
            DirectoryInfo directory = new DirectoryInfo(Path);
            directory.Rename(newName);
        }

        public void Delete()
        {
            Directory.Delete(Path, true);
        }
    }
}
