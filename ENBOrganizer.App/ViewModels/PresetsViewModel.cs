using ENBOrganizer.Domain.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Windows.Input;

namespace ENBOrganizer.App.ViewModels
{
    public class PresetsViewModel
    {
        private readonly PresetService _presetService;
        private readonly GameService _gameService;
        
        private ICommand AddBlankPresetCommand { get; set; }
        private ICommand ImportFolderCommand { get; set; }
        private ICommand ImportArchiveCommand { get; set; }
        private ICommand ImportActiveFilesCommand { get; set; }
        public List<TitledCommand> TitledCommands { get; set; }

        public PresetsViewModel()
        {
            AddBlankPresetCommand = new ActionCommand(AddBlank, () => true);
            ImportFolderCommand = new ActionCommand(ImportFolder, () => true);
            ImportArchiveCommand = new ActionCommand(ImportArchive, () => true);
            ImportActiveFilesCommand = new ActionCommand(ImportActiveFiles, () => true);

            TitledCommands = new List<TitledCommand>
            {
                new TitledCommand("Blank", "Create a blank preset", AddBlankPresetCommand),
                new TitledCommand("Import Folder", "Create a preset from a folder", ImportFolderCommand),
                new TitledCommand("Import Archive", "Create a preset an archive (.zip, .7z)", ImportArchiveCommand),
                new TitledCommand("Import Active Files", "Create a preset from preset files/folders currently in your game folder", ImportActiveFilesCommand)
            };

            _presetService = new PresetService();
            _gameService = ServiceSingletons.GameService;
        }

        private async void AddBlank()
        {
            string name = await DialogService.ShowInputDialog("Add Blank Preset", "Please enter a name for your preset:");
        }

        private async void ImportActiveFiles()
        {
            string name = await DialogService.ShowInputDialog("Import Active Files", "Please enter a name for your preset:");

            _presetService.CreatePresetFromActiveFiles(name, _gameService.CurrentGame);
        }

        private void ImportArchive()
        {
            throw new NotImplementedException();
        }

        private void ImportFolder()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog { Description = "Please select the preset folder..." };

            // TODO: let DialogService handle showing this dialog
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    _presetService.Import(folderBrowserDialog.SelectedPath, _gameService.CurrentGame);
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
