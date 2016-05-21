using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Exceptions;
using System.IO;

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
        
        public void CreateMasterListItems(DirectoryInfo directory)
        {
            foreach (FileSystemInfo fileSystemInfo in directory.GetFileSystemInfos())
            {
                MasterListItemType masterListItemType = fileSystemInfo is DirectoryInfo ? MasterListItemType.Directory : MasterListItemType.File;

                MasterListItem masterListItem = new MasterListItem(fileSystemInfo.Name, masterListItemType);

                try
                {
                    Add(masterListItem);
                }
                catch (DuplicateEntityException) { }
            }
        }
    }
}
