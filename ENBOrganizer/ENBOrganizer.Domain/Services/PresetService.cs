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

        // TODO: simplify this method
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

                    //DirectoryInfo directory = fileSystemInfo as DirectoryInfo;

                    //if (directory != null && directory.Exists)
                    //    directory.CopyTo(Path.Combine(preset.Directory.FullName, directory.Name));
                    //else
                    //{
                    //    FileInfo file = fileSystemInfo as FileInfo;
                    //    file.CopyTo(Path.Combine(preset.Directory.FullName, file.Name));
                    //}                                                
                }
                
                //List<string> gameDirectories = Directory.GetDirectories(preset.Game.ExecutableDirectory.FullName).ToList();
                //List<string> gameFiles = Directory.GetFiles(preset.Game.ExecutableDirectory.FullName).ToList();

                //foreach (MasterListItem masterListItem in masterListItems)
                //{
                //    string installedPath = Path.Combine(preset.Game.ExecutableDirectory.FullName, masterListItem.Name);

                //    if (masterListItem.Type == MasterListItemType.Directory) 
                //    {
                //        if (!gameDirectories.Contains(installedPath))
                //            continue;

                //        DirectoryInfo directory = new DirectoryInfo(installedPath);
                //        directory.CopyTo(Path.Combine(preset.Directory.FullName, directory.Name));
                //    }
                //    else if (gameFiles.Contains(installedPath))
                //    {
                //        FileInfo file = new FileInfo(installedPath);
                //        file.CopyTo(Path.Combine(preset.Directory.FullName, file.Name));
                //    }
                //}
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
            //foreach (Preset preset in Items.Where(preset => preset.IsEnabled))
            //{
            //    // Get installed path.
            //    foreach ()

            //    // Copy installed file/folder to preset.
            //}
        }
    }
}