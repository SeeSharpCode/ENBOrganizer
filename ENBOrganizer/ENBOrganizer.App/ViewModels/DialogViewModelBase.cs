using ENBOrganizer.App.Properties;
using ENBOrganizer.Domain.Entities;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using System.Windows.Input;

namespace ENBOrganizer.App.ViewModels
{
    public abstract class DialogViewModelBase : ViewModelBase
    {
        protected Game CurrentGame { get { return Settings.Default.CurrentGame; } }
        protected readonly DialogService _dialogService;

        public bool HasValidationError { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { Set(nameof(Name), ref _name, value); }
        }
        
        public DialogViewModelBase() : this(SimpleIoc.Default.GetInstance<DialogService>()) { }

        public DialogViewModelBase(DialogService dialogService)
        {
            _dialogService = dialogService;

            SaveCommand = new RelayCommand(Save, CanSave);
            CancelCommand = new RelayCommand(Close);
        }

        protected abstract void Save();
        protected abstract bool CanSave();
        protected abstract void Close();
    }
}