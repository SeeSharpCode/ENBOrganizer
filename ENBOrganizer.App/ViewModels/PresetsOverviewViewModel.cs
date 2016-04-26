using ENBOrganizer.App.Messages;
using ENBOrganizer.Domain;
using ENBOrganizer.Domain.Services;
using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Exceptions;
using ENBOrganizer.Util;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;

namespace ENBOrganizer.App.ViewModels
{
    public class PresetsOverviewViewModel : ViewModelBase
    {
        private readonly PresetService _presetService;
        private readonly PresetDetailViewModel _presetDetailViewModel;

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

        public ICommand SelectPresetCommand { get; private set; }
        public List<TitledCommand> TitledCommands { get; set; }

        public PresetsOverviewViewModel(PresetService presetService, PresetDetailViewModel presetDetailViewModel)
        {
            _presetService = presetService;
            _presetService.ItemsChanged += _presetService_ItemsChanged;

            _presetDetailViewModel = presetDetailViewModel;
            
            _addBlankPresetCommand = new RelayCommand(AddBlank, () => true);
            _importFolderCommand = new RelayCommand(ImportDirectory, () => true);
            _importArchiveCommand = new RelayCommand(ImportArchive, () => true);
            _importActiveFilesCommand = new RelayCommand(ImportActiveFiles, () => true);

            TitledCommands = new List<TitledCommand>
            {
                new TitledCommand("Blank", "Create a blank preset", _addBlankPresetCommand),
                new TitledCommand("Import Folder", "Create a preset from a folder", _importFolderCommand),
                new TitledCommand("Import Archive", "Create a preset an archive (.zip, .7z)", _importArchiveCommand),
                new TitledCommand("Import Active Files", "Create a preset from preset files/folders currently in your game folder", _importActiveFilesCommand)
            };


            SelectPresetCommand = new RelayCommand<Preset>((preset) => MessengerInstance.Send(new PresetNavigationMessage(preset)));

            MessengerInstance.Register<PropertyChangedMessage<Game>>(this, (message) => CurrentGame = message.NewValue);

            Presets = _presetService.GetByGame(_currentGame).ToObservableCollection();
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

            if (name == null || name.Trim() == string.Empty)
                return;

            try
            {
                _presetService.Add(new Preset(name, _currentGame));
            }
            catch (DuplicateEntityException exception)
            {
                await DialogService.ShowErrorDialog(exception.Message);
            }
        }

        private async void ImportActiveFiles()
        {
            // TODO: exception handling
            string name = await DialogService.ShowInputDialog("Import Active Files", "Please enter a name for your preset:");

            try
            {
                _presetService.ImportActiveFiles(new Preset(name, _currentGame));
            }
            catch (DuplicateEntityException exception)
            {
                await DialogService.ShowErrorDialog(exception.Message);
            }
        }

        private async void ImportArchive()
        {
            string archivePath = DialogService.PromptForFile("Please select a .zip file", "ZIP Files(*.zip) | *.zip");

            if (archivePath == null || archivePath.Trim() == string.Empty)
                return;

            try
            {
                _presetService.ImportArchive(archivePath, _currentGame);
            }
            catch (UnauthorizedAccessException exception)
            {
                await DialogService.ShowErrorDialog(exception.Message);
            }
            catch (NotSupportedException exception)
            {
                await DialogService.ShowErrorDialog(exception.Message);
            }
            catch (InvalidOperationException exception)
            {
                await DialogService.ShowErrorDialog(exception.Message);
            }
        }

        private void ImportDirectory()
        {
            string directoryPath = DialogService.PromptForFolder("Please select the preset folder...");

            if (directoryPath == string.Empty)
                return;

            try
            {
                _presetService.ImportDirectory(directoryPath, _currentGame);
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
