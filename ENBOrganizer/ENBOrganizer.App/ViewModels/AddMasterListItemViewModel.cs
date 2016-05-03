using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Services;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ENBOrganizer.App.ViewModels
{
    public class AddMasterListItemViewModel : ViewModelBase
    {
        private readonly DataService<MasterListItem> _masterListItemsService;

        private MasterListItemType _selectedMasterListItemType;

        public MasterListItemType SelectedMasterListItemType
        {
            get { return _selectedMasterListItemType; }
            set { Set("SelectedMasterListItemType", ref _selectedMasterListItemType, value); }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { Set("Name", ref _name, value); }
        }

        public IEnumerable<MasterListItemType> MasterListItemTypes
        {
            get { return Enum.GetValues(typeof(MasterListItemType)).Cast<MasterListItemType>(); }
        }
        
        public AddMasterListItemViewModel(DataService<MasterListItem> masterListItemsService)
        {
            _masterListItemsService = masterListItemsService;
        }
    }
}
