using ENBOrganizer.App.Properties;
using ENBOrganizer.Domain.Entities;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using MvvmValidation;
using System.ComponentModel;
using System.Windows.Input;
using System;

namespace ENBOrganizer.App.ViewModels
{
    public abstract class DialogViewModelBase : ViewModelBase, IDataErrorInfo
    {
        protected ValidationHelper _validator;
        protected DataErrorInfoAdapter _dataErrorInfoAdapter;
        protected Game CurrentGame { get { return Settings.Default.CurrentGame; } }
        protected readonly DialogService _dialogService;
        
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { Set(nameof(Name), ref _name, value); }
        }

        public string Error { get { return _dataErrorInfoAdapter.Error; } }

        public string this[string columnName] { get { return _dataErrorInfoAdapter[columnName]; } }

        public DialogViewModelBase() 
            : this(SimpleIoc.Default.GetInstance<DialogService>(), SimpleIoc.Default.GetInstance<ValidationHelper>()) { }

        public DialogViewModelBase(DialogService dialogService, ValidationHelper validationHelper)
        {
            _dialogService = dialogService;

            _validator = validationHelper;
            _dataErrorInfoAdapter = new DataErrorInfoAdapter(_validator);

            SaveCommand = new RelayCommand(Save, CanSave);
            CancelCommand = new RelayCommand(Close);
        }

        protected abstract void Save();
        protected abstract bool CanSave();
        protected abstract void Close();
    }
}