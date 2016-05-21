using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Exceptions;
using ENBOrganizer.Util.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace ENBOrganizer.Domain.Services
{
    // TODO: is a separate service needed for handling the file operations?
    public class PresetService : DataService<Preset>
    {
        private readonly MasterListService _masterListService;

        public PresetService(MasterListService masterListService)
        {
            _masterListService = masterListService;
        }

        public List<Preset> GetByGame(Game game)
        {
            return _repository.Items.Where(preset => preset.Game.Equals(game)).ToList();
        }
        
        public new void Add(Preset preset)
        {
            try
            {
                base.Add(preset);

                preset.Directory.Create();
            }
            catch (DuplicateEntityException)
            {
                throw;
            }
            catch (Exception) 
            {
                base.Delete(preset);

                throw;
            }
        }

        public void Import(Preset preset, string sourcePath)
        {
            try
            {
                base.Add(preset);

                DirectoryInfo sourceDirectory = new DirectoryInfo(sourcePath);

                if (sourceDirectory.Exists)
                    sourceDirectory.CopyTo(preset.Directory.FullName);
                else
                    ZipFile.ExtractToDirectory(sourcePath, preset.Directory.FullName);

                _masterListService.CreateMasterListItems(preset.Directory);
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

        public void Enable(Preset preset)
        {
            try
            {
                preset.Directory.CopyTo(preset.Game.DirectoryPath);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Disable(Preset preset)
        {
            try
            {
                foreach (FileSystemInfo fileSystemInfo in preset.Directory.GetFileSystemInfos())
                {
                    string installedPath = Path.Combine(preset.Game.DirectoryPath, fileSystemInfo.Name);

                    if (fileSystemInfo is DirectoryInfo && Directory.Exists(installedPath) && fileSystemInfo.Name != DirectoryNames.Data)
                        Directory.Delete(installedPath, true);
                    else if (File.Exists(installedPath))
                        File.Delete(installedPath);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DisableAll(Game currentGame)
        {
            try
            {
                foreach (MasterListItem masterListItem in _masterListService.GetAll())
                {
                    string installedPath = Path.Combine(currentGame.DirectoryPath, masterListItem.Name);

                    if (masterListItem.Type == MasterListItemType.File)
                    {
                        FileInfo file = new FileInfo(installedPath);

                        if (file.Exists)
                            file.Delete();
                    }
                    else
                    {
                        DirectoryInfo directory = new DirectoryInfo(installedPath);

                        if (directory.Exists)
                            directory.Delete(true);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public new void Delete(Preset preset)
        {
            preset.Directory.Delete(true);

            base.Delete(preset);
        }

        public void DeleteByGame(Game game)
        {
            List<Preset> presets = GetAll().Where(preset => preset.Game.Equals(game)).ToList();

            foreach (Preset preset in presets)
                Delete(preset);
        }
    }
}