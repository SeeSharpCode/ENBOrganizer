using ENBOrganizer.App.Messages;
using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Services;

namespace ENBOrganizer.App.ViewModels
{
    public class BinariesViewModel : PageViewModelBase<Binary>
    {
        protected override DialogName DialogName { get { return DialogName.AddBinary; } }
        
        public override string Name { get { return "Binaries"; } }

        public BinariesViewModel(FileSystemService<Binary> binaryService)
            : base(binaryService) { }
    }
}