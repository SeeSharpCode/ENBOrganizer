using ENBOrganizer.Model.Entities;
using ENBOrganizer.Util;
using ENBOrganizer.Util.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace ENBOrganizer.Domain.Services
{
    public class PresetService : DataService<Preset>
    {
        // TODO: remove this
        private readonly DataService<MasterListItem> _masterListItemService;

        public PresetService(DataService<MasterListItem> masterListItemService)
        {
            _masterListItemService = masterListItemService;
        }

        public List<Preset> GetByGame(Game game)
        {
            return _repository.Items.Where(preset => preset.Game.Equals(game)).ToList();
        }

        public void ChangeImage(ref Preset preset, string imageSource)
        {
            DirectoryInfo imagesDirectory = new DirectoryInfo("Images");

            if (!imagesDirectory.Exists)
                imagesDirectory.Create();

            string targetPath = Path.Combine("Images", preset.Game.Name + preset.Name + Path.GetExtension(imageSource));

            File.Copy(imageSource, targetPath);

            FileInfo file = new FileInfo(targetPath);

            preset.ImagePath = file.FullName;
        }

        public new void Add(Preset preset)
        {
            try
            {
                preset.Directory.Create();

                base.Add(preset);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
        }

        public void ImportArchive(string sourceFolderPath, Game game)
        {
            // TODO: exception handling
            Preset preset = new Preset(Path.GetFileNameWithoutExtension(sourceFolderPath), game);

            ZipFile.ExtractToDirectory(sourceFolderPath, preset.Directory.FullName);

            // TODO: following 2 lines are shared between ImportArchive and ImportFolder 
            base.Add(preset);

            CreateMasterListItemsFromPreset(preset);
        }

        public void ImportFolder(string sourceDirectoryPath, Game game)
        {
            DirectoryInfo sourceDirectory = new DirectoryInfo(sourceDirectoryPath);
            Preset preset = new Preset(sourceDirectory.Name, game);

            try
            {
                sourceDirectory.CopyTo(preset.Directory.FullName);

                base.Add(preset);

                CreateMasterListItemsFromPreset(preset);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (UnauthorizedAccessException)
            {
                _repository.Delete(preset);

                throw;
            }
            catch (PathTooLongException)
            {
                if (preset.Directory.Exists)
                    preset.Directory.DeleteRecursive();

                _repository.Delete(preset);

                throw;
            }
            catch (IOException)
            {
                _repository.Delete(preset);

                if (preset.Directory.Exists)
                    preset.Directory.DeleteRecursive();
                
                throw;
            }
        }

        private void CreateMasterListItemsFromPreset(Preset preset)
        {
            List<MasterListItem> masterListItems = _masterListItemService.GetAll();

            foreach (DirectoryInfo directory in preset.Directory.GetDirectories())
            {
                MasterListItem masterListItem = new MasterListItem(directory.Name, MasterListItemType.Directory);

                if (!masterListItems.Contains(masterListItem))
                    _masterListItemService.Add(masterListItem);
            }

            foreach (FileInfo file in preset.Directory.GetFiles())
            {
                MasterListItem masterListItem = new MasterListItem(file.Name, MasterListItemType.File);

                if (!masterListItems.Contains(masterListItem))
                    _masterListItemService.Add(masterListItem);
            }
        }

        public void ImportActiveFiles(Preset preset)
        {
            // TODO: exception handling
            Add(preset);

            List<MasterListItem> masterListItems = _masterListItemService.GetAll();
            List<string> gameDirectories = Directory.GetDirectories(preset.Game.DirectoryPath).ToList();
            List<string> gameFiles = Directory.GetFiles(preset.Game.DirectoryPath).ToList();
            
            foreach (MasterListItem masterListItem in masterListItems)
            {
                string installedPath = Path.Combine(preset.Game.DirectoryPath, masterListItem.Name);

                if (masterListItem.Type.Equals(MasterListItemType.Directory) && gameDirectories.Contains(installedPath))
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
        
        public void Install(Preset preset, Game currentGame)
        {
            foreach (FileInfo file in preset.Directory.GetFiles())
            {
                file.CopyTo(Path.Combine(currentGame.DirectoryPath, file.Name), true);
            }

            foreach (DirectoryInfo subdirectory in preset.Directory.GetDirectories())
            {
                if (!subdirectory.Name.EqualsIgnoreCase("Data")) // TODO: exception
                    subdirectory.CopyTo(Path.Combine(currentGame.DirectoryPath, subdirectory.Name));
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

        public void UninstallAll(Game currentGame)
        {
            foreach (MasterListItem masterListItem in _masterListItemService.GetAll())
            {
                string installedPath = Path.Combine(currentGame.DirectoryPath, masterListItem.Name);

                if (masterListItem.Type.Equals(MasterListItemType.File))
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
    }
}
