using ENBOrganizer.Domain;
using ENBOrganizer.Domain.Services;
using ENBOrganizer.Model.Entities;
using ENBOrganizer.UI.Views;
using ENBOrganizer.Util;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ENBOrganizer.UI.ViewModels
{
    public class MasterListViewModel
    {
        private readonly DataService<MasterListItem> _masterListItemService;

        public MasterListItem SelectedMasterListItem { get; set; }
        public ICommand AddMasterListItemCommand { get; set; }
        public ICommand DeleteMasterListItemCommand { get; set; }
        public ObservableCollection<MasterListItem> MasterListItems { get; set; }

        public MasterListViewModel()
        {
            _masterListItemService = ServiceSingletons.MasterListItemService;

            _masterListItemService.ItemsChanged += OnMasterListItemsChanged;

            MasterListItems = _masterListItemService.GetAll().ToObservableCollection();

            AddMasterListItemCommand = new ActionCommand(AddMasterListItem, () => true); // TODO: validation
            DeleteMasterListItemCommand = new ActionCommand(DeleteMasterListItem, () => true); // TODO: validation
        }

        private void OnMasterListItemsChanged(object sender, RepositoryChangedEventArgs eventArgs)
        {
            if (eventArgs.RepositoryActionType.Equals(RepositoryActionType.Added))
                MasterListItems.Add(eventArgs.Entity as MasterListItem);
            else
                MasterListItems.Remove(eventArgs.Entity as MasterListItem);

        }

        private void AddMasterListItem()
        {
            new AddMasterListItemView().ShowDialog();
        }

        private void DeleteMasterListItem()
        {
            _masterListItemService.Delete(SelectedMasterListItem);
        }
    }
}
