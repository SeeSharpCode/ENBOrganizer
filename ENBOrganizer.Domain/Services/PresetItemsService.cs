using ENBOrganizer.Model.Entities;
using ENBOrganizer.Util.IO;
using System.Collections.Generic;
using System.IO;

namespace ENBOrganizer.Domain.Services
{
    public class PresetItemsService
    {
        private readonly DataService<MasterListItem> _masterListItemService;

        public PresetItemsService(DataService<MasterListItem> masterListItemService)
        {
            _masterListItemService = masterListItemService;
        }

        public List<IPresetItem> GetPresetItems(string path)
        {
            List<IPresetItem> items = new List<IPresetItem>();

            DirectoryInfo rootDirectory = new DirectoryInfo(path);

            foreach (DirectoryInfo directory in rootDirectory.GetDirectories())
                items.Add(new PresetDirectory(directory.Name, directory.FullName, GetPresetItems(directory.FullName)));

            foreach (FileInfo file in rootDirectory.GetFiles())
                items.Add(new PresetFile(file.Name, file.FullName));

            return items;
        }

        public void CopyDirectoryAsPresetItem(IPresetItem destinationPresetItem, string path)
        {
            DirectoryInfo sourceDirectory = new DirectoryInfo(path);
            sourceDirectory.CopyTo(Path.Combine(destinationPresetItem.Path, sourceDirectory.Name));

            MasterListItem masterListItem = new MasterListItem(sourceDirectory.Name, MasterListItemType.Directory);

            if (!_masterListItemService.GetAll().Contains(masterListItem))
                _masterListItemService.Add(masterListItem);
        }

        public void CopyFileAsPresetItem(IPresetItem destinationPresetItem, string path)
        {
            File.Copy(path, Path.Combine(destinationPresetItem.Path, Path.GetFileName(path)));

            MasterListItem masterListItem = new MasterListItem(Path.GetFileName(path), MasterListItemType.File);

            if (!_masterListItemService.GetAll().Contains(masterListItem))
                _masterListItemService.Add(masterListItem);
        }
    }
}
