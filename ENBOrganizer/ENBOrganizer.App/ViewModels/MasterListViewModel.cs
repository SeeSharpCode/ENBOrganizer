using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Services;
using ENBOrganizer.Util;
using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ENBOrganizer.App.ViewModels
{
    public class MasterListViewModel : ViewModelBase
    {
        private readonly DataService<MasterListItem> _masterListItemService;

        public ICommand AddMasterListItemCommand { get; set; }
        public ICommand DeleteMasterListItemCommand { get; set; }

        public ObservableCollection<MasterListItem> MasterListItems { get; set; }

        public MasterListViewModel(DataService<MasterListItem> masterListItemsService)
        {
            _masterListItemService = masterListItemsService;

            MasterListItems = _masterListItemService.GetAll().ToObservableCollection();
        }
    }
}
