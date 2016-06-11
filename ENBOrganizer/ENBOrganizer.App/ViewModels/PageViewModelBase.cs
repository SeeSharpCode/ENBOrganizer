using ENBOrganizer.App.Messages;
using ENBOrganizer.App.Properties;
using ENBOrganizer.Domain;
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
        protected virtual DataService<TEntity> DataService { get; set; }
        protected readonly DialogService _dialogService;
        protected abstract DialogName DialogName { get; }
        
        public ObservableCollection<TEntity> Models { get; set; }
        
        public ICommand OpenAddDialogCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public Game CurrentGame
        {
            get { return Settings.Default.CurrentGame; }
            set
            {
                Settings.Default.CurrentGame = value;
                Settings.Default.Save();

                RaisePropertyChanged(nameof(CurrentGame));
            }
        }

        public PageViewModelBase(DataService<TEntity> dataService)
            : this(dataService, SimpleIoc.Default.GetInstance<DialogService>()) { }
        
        public PageViewModelBase(DataService<TEntity> dataService, DialogService dialogService)
        {
            DataService = dataService;
            DataService.ItemsChanged += _dataService_ItemsChanged;

            _dialogService = dialogService;

            OpenAddDialogCommand = new RelayCommand(() => _dialogService.ShowDialog(DialogName), CanAdd);
            DeleteCommand = new RelayCommand<TEntity>(entity => DataService.Delete(entity));

            Models = new ObservableCollection<TEntity>();

            PopulateModels();
        }

        protected virtual bool CanAdd()
        {
            return CurrentGame != null;
        }

        protected virtual void PopulateModels()
        {
            Models.Clear();
            Models.AddAll(DataService.GetAll());
        }

        protected virtual void _dataService_ItemsChanged(object sender, RepositoryChangedEventArgs eventArgs)
        {
            TEntity entity = eventArgs.Entity as TEntity;

            if (eventArgs.RepositoryActionType == RepositoryActionType.Added)
                Models.Add(entity);
            else
                Models.Remove(entity);
        }
    }
}
