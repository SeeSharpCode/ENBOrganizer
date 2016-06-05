using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Exceptions;
using ENBOrganizer.Util;
using System.IO;
using System.Linq;

namespace ENBOrganizer.Domain.Services
{
    public class MasterListService : DataService<MasterListItem>
    {
        public override void Add(MasterListItem masterListItem)
        {
            if (DirectoryNames.EssentialNames.Any(name => name.EqualsIgnoreCase(masterListItem.Name)))
                return;

            base.Add(masterListItem);
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
