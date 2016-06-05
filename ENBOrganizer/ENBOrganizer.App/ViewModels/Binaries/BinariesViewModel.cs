using ENBOrganizer.App.Messages;
using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Services;
using System;

namespace ENBOrganizer.App.ViewModels.Binaries
{
    public class BinariesViewModel : FileSystemViewModel<Binary>
    {
        protected override DialogName DialogName { get { return DialogName.AddBinary; } }
        protected override string DialogHostName { get { return "RenameBinaryDialog"; } }

        public BinariesViewModel(FileSystemService<Binary> binaryService)
            : base(binaryService) { }

        protected override async void Edit(Binary entity)
        {
            string newName = (string)await _dialogService.ShowInputDialog("Name", DialogHostName, entity.Name);

            if (newName == entity.Name)
                return;

            DataService.Rename(entity, newName);
        }
    }
}