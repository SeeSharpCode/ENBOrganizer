using ENBOrganizer.Util;

namespace ENBOrganizer.Model.Entities
{
    public enum MasterListItemType
    {
        Directory,
        File
    }

    public class MasterListItem : IEntity
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

            if (masterListItem == null)
                return false;

            return Name.EqualsIgnoreCase(masterListItem.Name) && Type.Equals(masterListItem.Type);
        }
    }
}
