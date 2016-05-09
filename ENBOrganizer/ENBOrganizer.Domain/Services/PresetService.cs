using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Exceptions;
using ENBOrganizer.Util;
using ENBOrganizer.Util.IO;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System;

namespace ENBOrganizer.Domain.Services
{
    public class PresetService : DataService<Preset>
    {
        private readonly DataService<MasterListItem> _masterListItemService;

        public PresetService(DataService<MasterListItem> masterListItemService)
        {
            _masterListItemService = masterListItemService;
        }

        public List<Preset> GetByGame(Game game)
        {
            return _repository.Items.Where(preset => preset.Game.Equals(game)).ToList();
        }

        public void SaveChanges()
        {
            _repository.SaveChanges();
        }

        public new void Add(Preset preset)
        {
            try
            {
                preset.Directory.Create();

                base.Add(preset);
            }
            catch (DuplicateEntityException)
            {
                throw;
            }
        }

        public void Disable(Preset preset)
        {
            // TODO: actual uninstall logic
            preset.IsEnabled = false;
            SaveChanges();
        }

        public void ImportArchive(string archivePath, Game game)
        {
            Preset preset = new Preset(Path.GetFileNameWithoutExtension(archivePath), game);

            ZipFile.ExtractToDirectory(archivePath, preset.Directory.FullName);

            // TODO: following 2 lines are shared between ImportArchive and ImportFolder 
            // and could we handle this with domain events?
            base.Add(preset);

            CreateMasterListItemsFromPreset(preset);
        }

        public void ImportDirectory(string sourceDirectoryPath, Game game)
        {
            DirectoryInfo sourceDirectory = new DirectoryInfo(sourceDirectoryPath);
            Preset preset = new Preset(sourceDirectory.Name, game);

            sourceDirectory.CopyTo(preset.Directory.FullName);

            base.Add(preset);

            CreateMasterListItemsFromPreset(preset);
        }

        public void ImportActiveFiles(Preset preset)
        {
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

        public void Enable(Preset preset)
        {
            foreach (FileInfo file in preset.Directory.GetFiles())
                file.CopyTo(Path.Combine(preset.Game.DirectoryPath, file.Name), true);

            foreach (DirectoryInfo subdirectory in preset.Directory.GetDirectories())
            {
                if (!subdirectory.Name.EqualsIgnoreCase("Data")) // TODO: exception
                    subdirectory.CopyTo(Path.Combine(preset.Game.DirectoryPath, subdirectory.Name));
            }

            preset.IsEnabled = true;
            SaveChanges();
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

        public void DisableAll(Game currentGame)
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

            foreach (Preset preset in _repository.Items)
                preset.IsEnabled = false;

            SaveChanges();
        }
    }
}
