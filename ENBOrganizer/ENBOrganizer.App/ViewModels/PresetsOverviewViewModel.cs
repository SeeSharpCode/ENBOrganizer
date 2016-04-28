using ENBOrganizer.App.Messages;
using ENBOrganizer.Domain;
using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Exceptions;
using ENBOrganizer.Domain.Services;
using ENBOrganizer.Util;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
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
        private readonly PresetDetailViewModel _presetDetailViewModel;
        private readonly DialogService _dialogService;

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

        public Game CurrentGame { get { return Properties.Settings.Default.CurrentGame; } }
        public ICommand SelectPresetCommand { get; private set; }
        public List<TitledCommand> TitledCommands { get; set; }

        public PresetsOverviewViewModel(PresetService presetService, PresetDetailViewModel presetDetailViewModel, DialogService dialogService)
        {
            _presetService = presetService;
            _presetService.ItemsChanged += _presetService_ItemsChanged;

            _dialogService = dialogService;

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

            Properties.Settings.Default.PropertyChanged += ApplicationSettings_PropertyChanged;

            Presets = _presetService.GetByGame(CurrentGame).ToObservableCollection();
        }

        private void ApplicationSettings_PropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == "CurrentGame")
                LoadPresets();
        }

        private void LoadPresets()
        {
            Presets.Clear();

            foreach (Preset preset in _presetService.GetByGame(CurrentGame))
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
            string name = await _dialogService.ShowInputDialog("Add Blank Preset", "Please enter a name for your preset:");

            if (name == null || name.Trim() == string.Empty)
                return;

            try
            {
                _presetService.Add(new Preset(name, CurrentGame));
            }
            catch (DuplicateEntityException exception)
            {
                await _dialogService.ShowErrorDialog(exception.Message);
            }
        }

        private async void ImportActiveFiles()
        {
            // TODO: exception handling
            string name = await _dialogService.ShowInputDialog("Import Active Files", "Please enter a name for your preset:");

            try
            {
                _presetService.ImportActiveFiles(new Preset(name, CurrentGame));
            }
            catch (DuplicateEntityException exception)
            {
                await _dialogService.ShowErrorDialog(exception.Message);
            }
        }

        private async void ImportArchive()
        {
            string archivePath = _dialogService.PromptForFile("Please select a .zip file", "ZIP Files(*.zip) | *.zip");

            if (archivePath == null || archivePath.Trim() == string.Empty)
                return;

            try
            {
                _presetService.ImportArchive(archivePath, CurrentGame);
            }
            catch (UnauthorizedAccessException exception)
            {
                await _dialogService.ShowErrorDialog(exception.Message);
            }
            catch (NotSupportedException exception)
            {
                await _dialogService.ShowErrorDialog(exception.Message);
            }
            catch (InvalidOperationException exception)
            {
                await _dialogService.ShowErrorDialog(exception.Message);
            }
        }

        private void ImportDirectory()
        {
            string directoryPath = _dialogService.PromptForFolder("Please select the preset folder...");

            if (directoryPath == string.Empty)
                return;

            try
            {
                _presetService.ImportDirectory(directoryPath, CurrentGame);
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
