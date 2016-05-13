using ENBOrganizer.App.Messages;
using ENBOrganizer.Domain;
using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Services;
using ENBOrganizer.Util;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ENBOrganizer.App.ViewModels
{
    public class MasterListViewModel : ViewModelBase
    {
        private readonly MasterListService _masterListService;
        private readonly DialogService _dialogService;

        public ICommand AddMasterListItemCommand { get; set; }
        public ICommand DeleteMasterListItemCommand { get; set; }
        public MasterListItem SelectedMasterListItem { get; set; }
        public ObservableCollection<MasterListItem> MasterListItems { get; set; }

        public MasterListViewModel(MasterListService masterListService, DialogService dialogService)
        {
            _masterListService = masterListService;
            _masterListService.ItemsChanged += _masterListItemService_ItemsChanged;

            _dialogService = dialogService;

            AddMasterListItemCommand = new RelayCommand(() => _dialogService.ShowDialog(Dialog.AddMasterListItem));
            DeleteMasterListItemCommand = new RelayCommand(() => _masterListService.Delete(SelectedMasterListItem), () => SelectedMasterListItem != null);

            MasterListItems = _masterListService.GetAll().ToObservableCollection();
        }

        private void _masterListItemService_ItemsChanged(object sender, RepositoryChangedEventArgs repositoryChangedEventArgs)
        {
            if (repositoryChangedEventArgs.RepositoryActionType == RepositoryActionType.Added)
                MasterListItems.Add(repositoryChangedEventArgs.Entity as MasterListItem);
            else
                MasterListItems.Remove(repositoryChangedEventArgs.Entity as MasterListItem);
        }
    }
}
