using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.Windows.Input;

namespace ENBOrganizer.App.ViewModels
{
    public class BinariesViewModel : ViewModelBase
    {
        private ICommand _importFolderCommand;
        private ICommand _importArchiveCommand;

        public List<TitledCommand> TitledCommands { get; set; }

        public BinariesViewModel()
        {
            TitledCommands = new List<TitledCommand>
            {
                new TitledCommand("Import Folder", "Create from a folder", _importFolderCommand),
                new TitledCommand("Import Archive", "Create from an archive (.zip, .7z)", _importArchiveCommand),
                new TitledCommand("Import Active Files", "Create from binary files currently in your game folder", _importArchiveCommand)
            };
        }
    }
}
