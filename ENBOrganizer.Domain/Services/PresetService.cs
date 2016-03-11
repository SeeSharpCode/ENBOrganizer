using ENBOrganizer.Data;
using ENBOrganizer.Model;
using ENBOrganizer.Model.Entities;
using ENBOrganizer.Util.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ENBOrganizer.Domain.Services
{
    public class PresetService
    {
        private readonly Repository<Preset> _presetRepository;

        public event EventHandler<RepositoryChangedEventArgs> PresetsChanged;
        public event EventHandler<RepositoryChangedEventArgs> PresetItemsChanged;

        public PresetService()
        {
            _presetRepository = new Repository<Preset>(RepositoryFileNames.Presets);
        }

        public List<Preset> GetAll()
        {
            return _presetRepository.GetAll();
        }

        public List<Preset> GetByGame(Game game)
        {
            List<Preset> presets = _presetRepository.GetAll();

            return presets.Where(preset => preset.Game.Equals(game)).ToList();
        }

        public void Add(Preset preset)
        {
            try
            {
                preset.Directory.Create();

                _presetRepository.Add(preset);

                RaisePresetsChanged(new RepositoryChangedEventArgs(RepositoryActionType.Added, preset));
            }
            catch (InvalidOperationException)
            {
                throw;
            }
        }
        
        public void Import(string sourceDirectoryPath, Game game)
        {
            DirectoryInfo sourceDirectory = new DirectoryInfo(sourceDirectoryPath);
            Preset preset = new Preset(sourceDirectory.Name, game);

            try
            {
                _presetRepository.Add(preset);

                sourceDirectory.CopyTo(preset.Directory.FullName);

                RaisePresetsChanged(new RepositoryChangedEventArgs(RepositoryActionType.Added, preset));
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (UnauthorizedAccessException)
            {
                _presetRepository.Delete(preset);

                throw;
            }
            catch (PathTooLongException)
            {
                if (preset.Directory.Exists)
                    preset.Directory.DeleteRecursive();

                _presetRepository.Delete(preset);

                throw;
            }
            catch (IOException)
            {
                _presetRepository.Delete(preset);

                if (preset.Directory.Exists)
                    preset.Directory.DeleteRecursive();
                
                throw;
            }
        }
        
        public List<PresetItem> GetPresetItems(string path)
        {
            List<PresetItem> items = new List<PresetItem>();

            DirectoryInfo rootDirectory = new DirectoryInfo(path);

            foreach (DirectoryInfo directory in rootDirectory.GetDirectories())
                items.Add(new PresetDirectory(directory.Name, directory.FullName, GetPresetItems(directory.FullName)));

            foreach (FileInfo file in rootDirectory.GetFiles())
                items.Add(new PresetFile(file.Name, file.FullName));

            return items;
        }

        public void RaisePresetsChanged(RepositoryChangedEventArgs eventArgs)
        {
            if (PresetsChanged != null)
                PresetsChanged(this, eventArgs);
        }

        public void RaisePresetItemsChanged(RepositoryChangedEventArgs eventArgs)
        {
            if (PresetItemsChanged != null)
                PresetItemsChanged(this, eventArgs);
        }

        public void DeleteItem(PresetItem presetItem)
        {
            if (presetItem.GetType() == typeof(PresetDirectory))
                Directory.Delete(presetItem.Path, true);
            else
                File.Delete(presetItem.Path);

            RaisePresetItemsChanged(new RepositoryChangedEventArgs(RepositoryActionType.Deleted, presetItem));
        }
    }
}
