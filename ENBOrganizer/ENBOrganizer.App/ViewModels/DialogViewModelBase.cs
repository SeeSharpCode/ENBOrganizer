using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using System.Windows.Input;

namespace ENBOrganizer.App.ViewModels
{
    public abstract class DialogViewModelBase : ViewModelBase
    {
        protected readonly DialogService _dialogService;

        private string _name;

        public string Name
        {
            get { return _name; }
            set { Set(nameof(Name), ref _name, value); }
        }

        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }

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
