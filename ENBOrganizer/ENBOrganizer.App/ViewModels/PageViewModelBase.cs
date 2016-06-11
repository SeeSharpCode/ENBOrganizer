using ENBOrganizer.App.Messages;
using ENBOrganizer.App.Properties;
using ENBOrganizer.Domain.Data;
using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ENBOrganizer.App.ViewModels
{
    public abstract class PageViewModelBase<TEntity> : ViewModelBase, IPageViewModel where TEntity : EntityBase
    {
        protected virtual DataService<ENBOrganizerContext, TEntity> DataService { get; set; }
        protected readonly DialogService _dialogService;
        protected abstract DialogName DialogName { get; }
        
        public ObservableCollection<TEntity> Models { get; set; }

        public virtual Game CurrentGame { get { return Settings.Default.CurrentGame; } }
        public ICommand OpenAddDialogCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public PageViewModelBase(DataService<ENBOrganizerContext, TEntity> dataService)
            : this(dataService, SimpleIoc.Default.GetInstance<DialogService>()) { }
        
        public PageViewModelBase(DataService<ENBOrganizerContext, TEntity> dataService, DialogService dialogService)
        {
            DataService = dataService;

            _dialogService = dialogService;

            OpenAddDialogCommand = new RelayCommand(() => _dialogService.ShowDialog(DialogName), CanAdd);
            DeleteCommand = new RelayCommand<TEntity>(entity => DataService.Delete(entity));

            Models = DataService.Items;
        }

        protected virtual bool CanAdd()
        {
            return true;
        }
    }
}
