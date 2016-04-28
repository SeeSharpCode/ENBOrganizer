using ENBOrganizer.Util.IO;
using System.IO;

namespace ENBOrganizer.Domain.Entities
{
    public class PresetFile : IPresetItem
    {
        private FileInfo _file;

        public string Name { get { return _file.Name; } }
        public string Path { get { return _file.FullName; } }

        public PresetFile(string path)
        {
            _file = new FileInfo(path);
        }

        public void Rename(string newName)
        {
            _file.Rename(newName);
        }

        public void Delete()
        {
            _file.Delete();
        }
    }
}