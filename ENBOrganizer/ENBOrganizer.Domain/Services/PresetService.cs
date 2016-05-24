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

        public List<Preset> GetByGame(Game game)
        {
            return _repository.Items.Where(preset => preset.Game.Equals(game)).ToList();
        }
        
        public void ImportActiveFiles(Preset preset)
        {
            try
            {
                Add(preset);

                List<MasterListItem> masterListItems = _masterListService.GetAll();

                List<string> gameDirectories = Directory.GetDirectories(preset.Game.DirectoryPath).ToList();
                List<string> gameFiles = Directory.GetFiles(preset.Game.DirectoryPath).ToList();

                foreach (MasterListItem masterListItem in masterListItems)
                {
                    string installedPath = Path.Combine(preset.Game.DirectoryPath, masterListItem.Name);

                    if (masterListItem.Type  == MasterListItemType.Directory && gameDirectories.Contains(installedPath))
                    {
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
    }
}