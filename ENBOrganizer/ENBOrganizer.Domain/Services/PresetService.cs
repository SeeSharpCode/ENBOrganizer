using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Exceptions;
using ENBOrganizer.Util;
using ENBOrganizer.Util.IO;
using System;
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
                
                foreach (FileSystemInfo fileSystemInfo in preset.Game.ExecutableDirectory.GetFileSystemInfos())
                {
                    if (!_masterListService.Items.Any(masterListItem => masterListItem.Name.EqualsIgnoreCase(fileSystemInfo.Name)))
                        continue;

                    fileSystemInfo.CopyTo(Path.Combine(preset.Directory.FullName, fileSystemInfo.Name));                                              
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

        public void SyncEnabledPresets()
        {
            foreach (Preset preset in Items.Where(preset => preset.IsEnabled))
            {
                foreach (FileSystemInfo presetFileSystemInfo in preset.Directory.GetFileSystemInfos())
                {
                    FileSystemInfo installedFileSystemInfo = 
                        preset.Game.ExecutableDirectory.GetFileSystemInfos()
                        .FirstOrDefault(gameFileSystemInfo => gameFileSystemInfo.Name.EqualsIgnoreCase(presetFileSystemInfo.Name) 
                        && !FileSystemNames.EssentialNames.Any(name => name.EqualsIgnoreCase(gameFileSystemInfo.Name)));

                    if (installedFileSystemInfo != null)
                        installedFileSystemInfo.CopyTo(presetFileSystemInfo.FullName);
                }
            }
        }
    }
}