using ENBOrganizer.App.Messages;
using ENBOrganizer.App.Properties;
using ENBOrganizer.Domain;
using ENBOrganizer.Domain.Data;
using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Services;
using ENBOrganizer.Util;
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
        
        public ObservableCollection<TEntity> Models { get { return DataService.Items; } }

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

            //Models = new ObservableCollection<TEntity>();

            //PopulateModels();
        }

        protected virtual bool CanAdd()
        {
            return CurrentGame != null;
        }

        // TODO: models are now exposed by service
        protected virtual void PopulateModels()
        {
            Models.Clear();
            Models.AddAll(DataService.Items);
        }
    }
}
