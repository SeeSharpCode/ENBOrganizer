using ENBOrganizer.App.Messages;
using ENBOrganizer.App.Properties;
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
    public abstract class PageViewModelBase<TEntity> : ViewModelBase, IPageViewModel where TEntity : EntityBase
    {
        protected virtual DataService<TEntity> DataService { get; set; }
        protected readonly DialogService _dialogService;
        protected abstract DialogName DialogName { get; }
        public abstract string Name { get; }

        private ObservableCollection<TEntity> _models;

        public ObservableCollection<TEntity> Models
        {
            get
            {
                if (_models == null)
                    _models = new ObservableCollection<TEntity>();

                return _models;
            }
        }

        private bool _isAddDialogOpen;

        public bool IsAddDialogOpen
        {
            get { return _isAddDialogOpen; }
            set { Set(nameof(IsAddDialogOpen), ref _isAddDialogOpen, value); }
        }

        public Game CurrentGame { get { return Settings.Default.CurrentGame; } }
        public ICommand OpenAddDialogCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public PageViewModelBase(DataService<TEntity> dataService, DialogService dialogService)
        {
            DataService = dataService;
            DataService.ItemsChanged += _dataService_ItemsChanged;

            _dialogService = dialogService;

            OpenAddDialogCommand = new RelayCommand(() => _dialogService.ShowDialog(DialogName), CanAdd);
            DeleteCommand = new RelayCommand<TEntity>(entity => DataService.Delete(entity));

            MessengerInstance.Register<DialogMessage>(this, OnDialogMessage);

            PopulateModels();
        }

        protected bool CanAdd()
        {
            return CurrentGame != null;
        }

        protected virtual void PopulateModels()
        {
            Models.Clear();
            Models.AddAll(DataService.GetAll().ToObservableCollection());
        }

        private void OnDialogMessage(DialogMessage message)
        {
            if (message.DialogName != DialogName)
                return;

            IsAddDialogOpen = message.DialogAction == DialogAction.Open;
        }

        protected void _dataService_ItemsChanged(object sender, RepositoryChangedEventArgs eventArgs)
        {
            TEntity entity = eventArgs.Entity as TEntity;

            if (eventArgs.RepositoryActionType == RepositoryActionType.Added)
                Models.Add(entity);
            else
                Models.Remove(entity);
        }
    }
}
