using ENBOrganizer.Util.IO;
using System.Collections.Generic;
using System.IO;

namespace ENBOrganizer.Domain.Entities
{
    public class PresetDirectory : IPresetItem
    {
        private DirectoryInfo _directory;

        public string Name { get { return _directory.Name; } }
        public string Path { get { return _directory.FullName; } }

        public List<IPresetItem> Items { get; set; }

        public PresetDirectory(string path, List<IPresetItem> presetItems)
        {
            _directory = new DirectoryInfo(path);

            Items = presetItems;
        }

        public void Rename(string newName)
        {
            _directory.Rename(newName);
        }

        public void Delete()
        {
            _directory.DeleteRecursive();
        }
    }
}
