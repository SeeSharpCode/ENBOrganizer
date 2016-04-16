using ENBOrganizer.Domain.Services;
using ENBOrganizer.Model.Entities;
using ENBOrganizer.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Forms;
using System.Windows.Input;

namespace ENBOrganizer.App.ViewModels
{
    public class PresetsOverviewViewModel : ObservableObject
    {
        private readonly PresetService _presetService;
        private readonly GameService _gameService;

        private readonly ICommand _addBlankPresetCommand;
        private readonly ICommand _importFolderCommand;
        private readonly ICommand _importArchiveCommand;
        private readonly ICommand _importActiveFilesCommand;

        public ObservableCollection<Preset> Presets { get; set; }
        public List<TitledCommand> TitledCommands { get; set; }

        public PresetsOverviewViewModel(PresetService presetService, GameService gameService)
        {
            _presetService = presetService;
            _gameService = gameService;

            _addBlankPresetCommand = new ActionCommand(AddBlank, () => true);
            _importFolderCommand = new ActionCommand(ImportFolder, () => true);
            _importArchiveCommand = new ActionCommand(ImportArchive, () => true);
            _importActiveFilesCommand = new ActionCommand(ImportActiveFiles, () => true);

            TitledCommands = new List<TitledCommand>
            {
                new TitledCommand("Blank", "Create a blank preset", _addBlankPresetCommand),
                new TitledCommand("Import Folder", "Create a preset from a folder", _importFolderCommand),
                new TitledCommand("Import Archive", "Create a preset an archive (.zip, .7z)", _importArchiveCommand),
                new TitledCommand("Import Active Files", "Create a preset from preset files/folders currently in your game folder", _importActiveFilesCommand)
            };

            Presets = _presetService.GetByGame(_gameService.CurrentGame).ToObservableCollection();
        }

        private async void AddBlank()
        {
            // TODO: exception handling
            string name = await DialogService.ShowInputDialog("Add Blank Preset", "Please enter a name for your preset:");

            _presetService.Add(new Preset(name, _gameService.CurrentGame));
        }

        private async void ImportActiveFiles()
        {
            // TODO: exception handling
            string name = await DialogService.ShowInputDialog("Import Active Files", "Please enter a name for your preset:");

            _presetService.ImportActiveFiles(new Preset(name, _gameService.CurrentGame));
        }

        private void ImportArchive()
        {
            string archivePath = DialogService.ShowOpenFileDialog("Please select a .zip file", "ZIP Files(*.zip) | *.zip");

            if (archivePath == string.Empty)
                return;

            _presetService.ImportArchive(archivePath, _gameService.CurrentGame);
        }

        private void ImportFolder()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog { Description = "Please select the preset folder..." };

            // TODO: let DialogService handle showing this dialog
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    _presetService.ImportFolder(folderBrowserDialog.SelectedPath, _gameService.CurrentGame);
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
}
