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
    public class PageViewModelBase<TEntity> : ViewModelBase where TEntity : EntityBase
    {
        protected readonly DataService<TEntity> _dataService;
        protected readonly DialogService _dialogService;
        private readonly DialogName _dialogName;

        public ObservableCollection<TEntity> Models { get; set; }

        private bool _isAddDialogOpen;

        public bool IsAddDialogOpen
        {
            get { return _isAddDialogOpen; }
            set { Set(nameof(IsAddDialogOpen), ref _isAddDialogOpen, value); }
        }

        public string Name { get; set; }
        public Game CurrentGame { get { return Settings.Default.CurrentGame; } }
        public ICommand OpenAddDialogCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public PageViewModelBase(DataService<TEntity> dataService, DialogService dialogService, DialogName dialogName, string name)
        {
            _dataService = dataService;
            _dataService.ItemsChanged += _dataService_ItemsChanged;

            _dialogService = dialogService;

            _dialogName = dialogName;

            Name = name;

            OpenAddDialogCommand = new RelayCommand(() => _dialogService.ShowDialog(_dialogName), CanAdd);
            DeleteCommand = new RelayCommand<TEntity>(entity => _dataService.Delete(entity));

            MessengerInstance.Register<DialogMessage>(this, OnDialogMessage);

            PopulateModels();
        }

        protected bool CanAdd()
        {
            return CurrentGame != null;
        }

        protected void PopulateModels()
        {
            if (Models == null)
                Models = new ObservableCollection<TEntity>();

            Models.Clear();
            Models.AddAll(_dataService.GetAll().ToObservableCollection());
        }

        private void OnDialogMessage(DialogMessage message)
        {
            if (message.DialogName != _dialogName)
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
