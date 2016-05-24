using ENBOrganizer.App.Messages;
using ENBOrganizer.Domain;
using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ENBOrganizer.App.ViewModels
{
    public abstract class PageViewModelBase<TEntity> : ViewModelBase where TEntity : EntityBase
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

        public abstract string Name { get; }
        public ICommand OpenAddDialogCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public PageViewModelBase(DataService<TEntity> dataService, DialogService dialogService, DialogName dialogName)
        {
            _dataService = dataService;
            _dataService.ItemsChanged += _dataService_ItemsChanged;

            _dialogService = dialogService;

            _dialogName = dialogName;

            OpenAddDialogCommand = new RelayCommand(() => _dialogService.ShowDialog(_dialogName), CanSave);
            DeleteCommand = new RelayCommand<TEntity>(entity => _dataService.Delete(entity));

            MessengerInstance.Register<DialogMessage>(this, OnDialogMessage);

            PopulateModels();
        }

        protected abstract bool CanSave();

        protected abstract void PopulateModels();

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
