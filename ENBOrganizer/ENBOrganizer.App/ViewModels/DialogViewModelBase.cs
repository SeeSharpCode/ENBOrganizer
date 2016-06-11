using ENBOrganizer.Util.IO;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using MvvmValidation;
using System.ComponentModel;
using System.Windows.Input;

namespace ENBOrganizer.App.ViewModels
{
    public abstract class DialogViewModelBase : ViewModelBase, IDataErrorInfo
    {
        protected ValidationHelper _validator;
        protected DataErrorInfoAdapter _dataErrorInfoAdapter;
        protected readonly DialogService _dialogService;
        protected readonly SettingsService _settingsService;
        
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        
        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                _validator.Validate(() => Name);
                RaisePropertyChanged(nameof(Name));
            }
        }

        public string Error { get { return _dataErrorInfoAdapter.Error; } }

        public string this[string columnName] { get { return _dataErrorInfoAdapter[columnName]; } }

        public DialogViewModelBase() 
            : this(SimpleIoc.Default.GetInstance<DialogService>(), 
                  SimpleIoc.Default.GetInstance<ValidationHelper>(), 
                  SimpleIoc.Default.GetInstance<SettingsService>()) { }

        public DialogViewModelBase(DialogService dialogService, ValidationHelper validationHelper, SettingsService settingService)
        {
            _validator = validationHelper;
            _dataErrorInfoAdapter = new DataErrorInfoAdapter(_validator);

            _dialogService = dialogService;
            _settingsService = settingService;

            SaveCommand = new RelayCommand(Save, () => _validator.ValidateAll().IsValid);
            CancelCommand = new RelayCommand(Close);

            SetupValidationRules();
            _validator.ValidateAll();
        }
        
        protected virtual void SetupValidationRules()
        {
            _validator.RemoveAllRules();

            _validator.AddRequiredRule(() => Name, "Name is required.");
            _validator.AddRule(() => Name, () => RuleResult.Assert(PathUtil.IsValidFileSystemName(Name), "Name contains invalid character(s)."));
        }

        protected abstract void Save();
        protected abstract void Close();
    }
}