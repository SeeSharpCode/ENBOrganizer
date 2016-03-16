using ENBOrganizer.Util.IO;
using System;
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

        public void Rename(string newName)
        {
            FileInfo file = new FileInfo(Path);
            file.Rename(newName);
        }

        public void Delete()
        {
            File.Delete(Path);
        }
    }
}