using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Exceptions;
using ENBOrganizer.Util.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ENBOrganizer.Domain.Services
{
    public class PresetService : FileSystemService<Preset>
    {
        public PresetService(MasterListService masterListService) : base(masterListService) { }        

        public void ImportInstalledFiles(Preset preset)
        {
            try
            {
                base.Add(preset);
                preset.Directory.Create();

                List<MasterListItem> masterListItems = _masterListService.GetAll();

                List<string> gameDirectories = Directory.GetDirectories(preset.Game.ExecutableDirectory.FullName).ToList();
                List<string> gameFiles = Directory.GetFiles(preset.Game.ExecutableDirectory.FullName).ToList();

                // TODO: reverse this by reading all game files/folders and checking if they exist in the master list?
                // TODO: how is this affected by multiple games?
                foreach (MasterListItem masterListItem in masterListItems)
                {
                    string installedPath = Path.Combine(preset.Game.ExecutableDirectory.FullName, masterListItem.Name);

                    if (masterListItem.Type == MasterListItemType.Directory) 
                    {
                        if (!gameDirectories.Contains(installedPath))
                            continue;

                        DirectoryInfo directory = new DirectoryInfo(installedPath);
                        directory.CopyTo(Path.Combine(preset.Directory.FullName, directory.Name));
                    }
                    else if (gameFiles.Contains(installedPath))
                    {
                        FileInfo file = new FileInfo(installedPath);
                        file.CopyTo(Path.Combine(preset.Directory.FullName, file.Name));
                    }
                }
            }
            catch (DuplicateEntityException)
            {
                throw;
            }
            catch (Exception)
            {
                Delete(preset);

                throw;
            }
        }

        public void RefreshEnabledPresets()
        {
            foreach (Preset preset in GetAll().Where(preset => preset.IsEnabled))
            {
                // Get installed path.
                // Copy installed file/folder to preset.
            }
        }
    }
}