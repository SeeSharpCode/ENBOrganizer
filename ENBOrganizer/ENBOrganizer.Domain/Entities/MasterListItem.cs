using ENBOrganizer.Util;

namespace ENBOrganizer.Domain.Entities
{
    public enum MasterListItemType
    {
        PresetDirectory,
        PresetFile,
        BinaryDirectory,
        BinaryFile
    }

#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class MasterListItem : IEntity
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        public string Name { get; set; }
        public MasterListItemType Type { get; set; }

        public MasterListItem() { }

        public MasterListItem(string name, MasterListItemType type)
        {
            Name = name;
            Type = type;
        }

        public override bool Equals(object other)
        {
            MasterListItem masterListItem = other as MasterListItem;

            return masterListItem != null ? Name.EqualsIgnoreCase(masterListItem.Name) && Type.Equals(masterListItem.Type) : false;
        }
    }
}
