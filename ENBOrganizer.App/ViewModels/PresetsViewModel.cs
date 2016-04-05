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

        public List<TitledCommand> TitledCommands { get; set; }
        private ICommand AddBlankPresetCommand { get; set; }
        private ICommand ImportPresetCommand { get; set; }

        public PresetsViewModel()
        {
            AddBlankPresetCommand = new ActionCommand(AddBlank, () => true);
            ImportPresetCommand = new ActionCommand(ImportPreset, () => true);
            TitledCommands = new List<TitledCommand> { new TitledCommand("Blank", AddBlankPresetCommand), new TitledCommand("Import", ImportPresetCommand) };

            _presetService = new PresetService();
            _gameService = ServiceSingletons.GameService;
        }

        private async void AddBlank()
        {
            string name = await DialogService.ShowInputDialog("Add Blank Preset", "Please enter a name for your preset:");
        }

        private void ImportPreset()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog { Description = "Please select the preset folder..." };

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
