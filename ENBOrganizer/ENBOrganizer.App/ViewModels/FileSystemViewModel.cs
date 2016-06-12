using ENBOrganizer.App.Properties;
using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Services;
using ENBOrganizer.Util;
using GalaSoft.MvvmLight.CommandWpf;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using System;

namespace ENBOrganizer.App.ViewModels
{
    public abstract class FileSystemViewModel<TEntity> : PageViewModelBase<TEntity> where TEntity : FileSystemEntity
    {
        protected new FileSystemService<TEntity> DataService { get { return (FileSystemService<TEntity>)base.DataService; } }
        protected abstract string DialogHostName { get; }

        public ICommand ViewFilesCommand { get; set; }
        public ICommand ChangeStateCommand { get; set; }
        public ICommand DisableAllCommand { get; set; }
        public ICommand EditCommand { get; set; }

        public FileSystemViewModel(DataService<TEntity> dataService) : base(dataService)
        {
            ViewFilesCommand = new RelayCommand<TEntity>(entity => Process.Start(entity.Directory.FullName));
            ChangeStateCommand = new RelayCommand<TEntity>(OnStateChanged);
            DisableAllCommand = new RelayCommand(DisableAll, CanDisableAll);
            EditCommand = new RelayCommand<TEntity>(Edit);

            SettingsService.PropertyChanged += Default_PropertyChanged;
        }

        protected abstract void Edit(TEntity entity);

        private bool CanDisableAll()
        {
            return Models.Any();
        }

        protected override bool CanAdd()
        {
            return SettingsService.CurrentGame != null;
        }

        private void OnStateChanged(TEntity entity)
        {
            entity.ChangeState();
            DataService.SaveChanges();
        }

        private void DisableAll()
        {
            DataService.DisableAll(SettingsService.CurrentGame);
        }

        private void Default_PropertyChanged(object sender, PropertyChangedEventArgs eventArgs)
        {
            if (eventArgs.PropertyName == "CurrentGame")
                PopulateModels();
        }

        protected override void PopulateModels()
        {
            Models.Clear();
            Models.AddAll(DataService.GetByGame(SettingsService.CurrentGame));
        }
    }
}