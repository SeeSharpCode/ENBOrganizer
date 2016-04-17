using ENBOrganizer.Domain;
using ENBOrganizer.Domain.Services;
using ENBOrganizer.Model.Entities;
using ENBOrganizer.Util;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;

namespace ENBOrganizer.App.ViewModels
{
    public class PresetsOverviewViewModel : ViewModelBase
    {
        private readonly PresetService _presetService;

        private readonly ICommand _addBlankPresetCommand;
        private readonly ICommand _importFolderCommand;
        private readonly ICommand _importArchiveCommand;
        private readonly ICommand _importActiveFilesCommand;
        
        private ObservableCollection<Preset> _presets;

        public ObservableCollection<Preset> Presets
        {
            get { return _presets; }
            set
            {
                _presets = value;
                RaisePropertyChanged("Presets");
            }
        }

        private Game _currentGame;

        public Game CurrentGame
        {
            get { return _currentGame; }
            set
            {
                _currentGame = value;
                LoadPresets();
            }
        }

        public ICommand GoToPresetDetailCommand { get; private set; }
        public List<TitledCommand> TitledCommands { get; set; }

        public PresetsOverviewViewModel(PresetService presetService)
        {
            _presetService = presetService;
            _presetService.ItemsChanged += _presetService_ItemsChanged;
            
            _addBlankPresetCommand = new RelayCommand(AddBlank, () => true);
            _importFolderCommand = new RelayCommand(ImportFolder, () => true);
            _importArchiveCommand = new RelayCommand(ImportArchive, () => true);
            _importActiveFilesCommand = new RelayCommand(ImportActiveFiles, () => true);

            TitledCommands = new List<TitledCommand>
            {
                new TitledCommand("Blank", "Create a blank preset", _addBlankPresetCommand),
                new TitledCommand("Import Folder", "Create a preset from a folder", _importFolderCommand),
                new TitledCommand("Import Archive", "Create a preset an archive (.zip, .7z)", _importArchiveCommand),
                new TitledCommand("Import Active Files", "Create a preset from preset files/folders currently in your game folder", _importActiveFilesCommand)
            };

            GoToPresetDetailCommand = new RelayCommand<Preset>((preset) => MessengerInstance.Send(new GoToPresetDetailMessage(preset)));

            MessengerInstance.Register<PropertyChangedMessage<Game>>(this, (message) => CurrentGame = message.NewValue);

            Presets = _presetService.GetByGame(_currentGame).ToObservableCollection();
        }

        private void _gamesViewModel_PropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == "CurrentGame")
                LoadPresets();
        }

        private void LoadPresets()
        {
            Presets.Clear();

            foreach (Preset preset in _presetService.GetByGame(_currentGame))
                Presets.Add(preset);
        }

        private void _presetService_ItemsChanged(object sender, RepositoryChangedEventArgs repositoryChangedEventArgs)
        {
            if (repositoryChangedEventArgs.RepositoryActionType == RepositoryActionType.Added)
                Presets.Add(repositoryChangedEventArgs.Entity as Preset);
            else
                Presets.Remove(repositoryChangedEventArgs.Entity as Preset);
        }

        private async void AddBlank()
        {
            // TODO: exception handling
            string name = await DialogService.ShowInputDialog("Add Blank Preset", "Please enter a name for your preset:");

            _presetService.Add(new Preset(name, _currentGame));
        }

        private async void ImportActiveFiles()
        {
            // TODO: exception handling
            string name = await DialogService.ShowInputDialog("Import Active Files", "Please enter a name for your preset:");

            _presetService.ImportActiveFiles(new Preset(name, _currentGame));
        }

        private void ImportArchive()
        {
            string archivePath = DialogService.PromptForFile("Please select a .zip file", "ZIP Files(*.zip) | *.zip");

            if (archivePath == string.Empty)
                return;

            _presetService.ImportArchive(archivePath, _currentGame);
        }

        private void ImportFolder()
        {
            string folderPath = DialogService.PromptForFolder("Please select the preset folder...");

            if (folderPath == string.Empty)
                return;

            try
            {
                _presetService.ImportFolder(folderPath, _currentGame);
            }
            catch (InvalidOperationException exception)
            {
                // TODO: MessageBoxUtil.ShowError(exception.Message);
            }
            catch (UnauthorizedAccessException exception)
            {
                //MessageBoxUtil.ShowError("Not all files were imported." + Environment.NewLine + Environment.NewLine + exception.Message);
            }
            catch (PathTooLongException exception)
            {
                //MessageBoxUtil.ShowError(exception.Message);
            }
            catch (IOException exception)
            {
                //MessageBoxUtil.ShowError("Not all files were imported." + Environment.NewLine + Environment.NewLine + exception.Message);
            }
        }
    }
}
