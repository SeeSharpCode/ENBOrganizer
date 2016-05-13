using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Exceptions;

namespace ENBOrganizer.Domain.Services
{
    public class MasterListService : DataService<MasterListItem>
    {
        public new void Add(MasterListItem masterListItem)
        {
            try
            {
                if (masterListItem.Name == DirectoryNames.Data && masterListItem.Type == MasterListItemType.Directory)
                    return;

                base.Add(masterListItem);
            }
            catch (DuplicateEntityException)
            {
                throw;
            }
        }
    }
}
