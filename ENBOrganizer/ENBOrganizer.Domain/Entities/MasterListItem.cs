using ENBOrganizer.Util;

namespace ENBOrganizer.Domain.Entities
{
    public enum MasterListItemType
    {
        File,
        Directory
    }

#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class MasterListItem : EntityBase
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        public MasterListItemType Type { get; set; }

        public MasterListItem() { }

        public MasterListItem(string name, MasterListItemType type)
            : base(name)
        {
            Type = type;
        }

        public override bool Equals(object other)
        {
            MasterListItem masterListItem = other as MasterListItem;

            return masterListItem != null ? ID.EqualsIgnoreCase(masterListItem.ID) : false;
        }
    }
}
