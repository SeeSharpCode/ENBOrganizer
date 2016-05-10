using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Exceptions;
using ENBOrganizer.Domain.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace ENBOrganizer.App.ViewModels
{
    public class AddMasterListItemViewModel : ViewModelBase
    {
        private readonly DataService<MasterListItem> _masterListItemsService;
        private readonly DialogService _dialogService;

        public ICommand AddMasterListItemCommand { get; set; }

        private MasterListItemType _selectedMasterListItemType;

        public MasterListItemType SelectedMasterListItemType
        {
            get { return _selectedMasterListItemType; }
            set { Set(nameof(SelectedMasterListItemType), ref _selectedMasterListItemType, value); }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { Set(nameof(Name), ref _name, value); }
        }

        public IEnumerable<MasterListItemType> MasterListItemTypes
        {
            get { return Enum.GetValues(typeof(MasterListItemType)).Cast<MasterListItemType>(); }
        }
        
        public AddMasterListItemViewModel(DataService<MasterListItem> masterListItemsService, DialogService dialogService)
        {
            _masterListItemsService = masterListItemsService;
            _dialogService = dialogService;

            AddMasterListItemCommand = new RelayCommand(AddMasterListItem, () => !string.IsNullOrWhiteSpace(Name));
        }

        private async void AddMasterListItem()
        {
            try
            {
                _masterListItemsService.Add(new MasterListItem(Name, SelectedMasterListItemType));
            }
            catch (DuplicateEntityException exception)
            {
                await _dialogService.ShowErrorDialog(exception.Message);
            }
            finally
            {
                Name = string.Empty;

                _dialogService.CloseDialog();
            }
        }
    }
}
