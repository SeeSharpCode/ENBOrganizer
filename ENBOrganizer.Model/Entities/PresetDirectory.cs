using System.Collections.Generic;

namespace ENBOrganizer.Model.Entities
{
    public class PresetDirectory : PresetItem
    {
        public List<PresetItem> Items { get; set; }

        public PresetDirectory(string name, string path, List<PresetItem> presetItems)
        {
            Name = name;
            Path = path;
            Items = presetItems;
        }
    }
}
