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
        private readonly MasterListService _masterListService;
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
        
        public AddMasterListItemViewModel(MasterListService masterListService, DialogService dialogService)
        {
            _masterListService = masterListService;
            _dialogService = dialogService;

            AddMasterListItemCommand = new RelayCommand(AddMasterListItem, () => !string.IsNullOrWhiteSpace(Name));
        }

        private void AddMasterListItem()
        {
            try
            {
                _masterListService.Add(new MasterListItem(Name, SelectedMasterListItemType));
            }
            catch (DuplicateEntityException exception)
            {
                _dialogService.ShowErrorDialog(exception.Message);
            }
            finally
            {
                Name = string.Empty;

                _dialogService.CloseDialog();
            }
        }
    }
}
