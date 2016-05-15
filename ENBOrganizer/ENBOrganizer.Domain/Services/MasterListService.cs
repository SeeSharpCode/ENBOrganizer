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
                if (masterListItem.Name == DirectoryNames.Data && masterListItem.Type == MasterListItemType.PresetDirectory)
                    return;

                base.Add(masterListItem);
            }
            catch (DuplicateEntityException)
            {
                throw;
            }
        }
        
        public void CreateMasterListItems(Preset preset)
        {
            foreach (FileSystemInfo fileSystemInfo in preset.Directory.GetFileSystemInfos())
            {
                MasterListItemType masterListItemType = fileSystemInfo is DirectoryInfo ? MasterListItemType.PresetDirectory : MasterListItemType.PresetFile;

                MasterListItem masterListItem = new MasterListItem(fileSystemInfo.Name, masterListItemType);

                try
                {
                    Add(masterListItem);
                }
                catch (DuplicateEntityException) { }
            }
        }

        public void CreateMasterListItems(Binary binary)
        {
            foreach (FileSystemInfo fileSystemInfo in binary.Directory.GetFileSystemInfos())
            {
                MasterListItemType masterListItemType = fileSystemInfo is DirectoryInfo ? MasterListItemType.BinaryDirectory : MasterListItemType.BinaryFile;

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
