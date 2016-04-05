using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace ENBOrganizer.App.ViewModels
{
    public class PresetsViewModel
    {
        public List<TitledCommand> TitledCommands { get; set; }

        private ICommand AddBlankPresetCommand { get; set; }
        private ICommand ImportPresetCommand { get; set; }

        public PresetsViewModel()
        {
            AddBlankPresetCommand = new ActionCommand(AddBlank, () => true);
            ImportPresetCommand = new ActionCommand(ImportPreset, () => true);

            TitledCommands = new List<TitledCommand> { new TitledCommand("New Blank", AddBlankPresetCommand), new TitledCommand("Import", ImportPresetCommand) };
        }

        private void AddBlank()
        {
            throw new NotImplementedException();
        }

        private void ImportPreset()
        {
            throw new NotImplementedException();
        }
    }
}
