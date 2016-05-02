using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Services;
using GalaSoft.MvvmLight;

namespace ENBOrganizer.App.ViewModels
{
    public class AddMasterListItemViewModel : ViewModelBase
    {
        private readonly DataService<MasterListItem> _masterListItemsService;

        private string _name;

        public string Name
        {
            get { return _name; }
            set { Set("Name", ref _name, value); }
        }

        public AddMasterListItemViewModel(DataService<MasterListItem> masterListItemsService)
        {
            _masterListItemsService = masterListItemsService;
        }
    }
}
