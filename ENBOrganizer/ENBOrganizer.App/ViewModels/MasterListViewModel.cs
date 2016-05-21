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
    public class MasterListViewModel : ViewModelBase, IPageViewModel
    {
        private readonly MasterListService _masterListService;
        private readonly DialogService _dialogService;

        public string Name { get { return "Master List"; } }
        public ICommand OpenAddMasterListItemDialogCommand { get; set; }
        public ICommand DeleteMasterListItemCommand { get; set; }
        public ObservableCollection<MasterListItem> MasterListItems { get; set; }

        private bool _isAddMasterListDialogOpen;

        public bool IsAddMasterListDialogOpen
        {
            get { return _isAddMasterListDialogOpen; }
            set { Set(nameof(IsAddMasterListDialogOpen), ref _isAddMasterListDialogOpen, value); }
        }

        public MasterListViewModel(MasterListService masterListService, DialogService dialogService)
        {
            _masterListService = masterListService;
            _masterListService.ItemsChanged += _masterListItemService_ItemsChanged;

            _dialogService = dialogService;

            OpenAddMasterListItemDialogCommand = new RelayCommand(() => _dialogService.ShowDialog(DialogName.AddMasterListItem));
            DeleteMasterListItemCommand = new RelayCommand<MasterListItem>(masterListItem => _masterListService.Delete(masterListItem));

            MessengerInstance.Register<DialogMessage>(this, OnDialogMessage);

            MasterListItems = _masterListService.GetAll().ToObservableCollection();
        }

        private void OnDialogMessage(DialogMessage dialogMessage)
        {
            if (dialogMessage.DialogName != DialogName.AddMasterListItem)
                return;

            IsAddMasterListDialogOpen = dialogMessage.DialogAction == DialogAction.Open;
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
