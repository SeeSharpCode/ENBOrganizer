using ENBOrganizer.App.Messages;
using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Services;
using ENBOrganizer.Util;
using System.Linq;

namespace ENBOrganizer.App.ViewModels.Binaries
{
    public class BinariesViewModel : FileSystemViewModel<Binary>
    {
        protected override DialogName DialogName { get { return DialogName.AddBinary; } }
        protected override string DialogHostName { get { return "RenameBinaryDialog"; } }

        public BinariesViewModel(FileSystemService<Binary> binaryService)
            : base(binaryService) { }

        protected override async void Edit(Binary binary)
        {
            string newName = ((string)await _dialogService.ShowInputDialog("Please enter a name for this binary:", DialogHostName, binary.Name)).Trim();

            if (!ShouldEdit(binary, newName))
                return;

            DataService.Rename(binary, newName);
        }

        private bool ShouldEdit(Binary binary, string newName)
        {
            if (binary.Name.EqualsIgnoreCase(newName))
                return false;

            if (DataService.GetByGame(SettingsService.CurrentGame).Any(b => b.Name.EqualsIgnoreCase(newName)))
            {
                _dialogService.ShowErrorDialog("A binary with this name already exists.");
                return false;
            }
                        
            return true;
        }
    }
}