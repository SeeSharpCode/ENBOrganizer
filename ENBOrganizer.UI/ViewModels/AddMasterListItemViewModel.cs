using ENBOrganizer.Domain.Services;
using ENBOrganizer.Model.Entities;
using System.Collections.Generic;
using System.Windows.Input;

namespace ENBOrganizer.UI.ViewModels
{
    public class AddMasterListItemViewModel
    {
        private readonly DataService<MasterListItem> _masterListItemService;

        public string Name { get; set; }
        public MasterListItemType Type { get; set; }
        public ICommand AddMasterListItemCommand { get; set; }

        public Dictionary<MasterListItemType, string> MasterListItemTypes
        {
            get
            {
                return new Dictionary<MasterListItemType, string>
                {
                    { MasterListItemType.Directory, "Directory" },
                    { MasterListItemType.File, "File" }
                };
            }
        }
        
        public AddMasterListItemViewModel()
        {
            _masterListItemService = ServiceSingletons.MasterListItemService;

            AddMasterListItemCommand = new ActionCommand(AddMasterListItem, () => true); // TODO: validation
        }

        private void AddMasterListItem()
        {
            _masterListItemService.Add(new MasterListItem(Name, Type));
        }
    }
}
