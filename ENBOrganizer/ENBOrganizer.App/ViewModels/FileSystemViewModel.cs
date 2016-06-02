using ENBOrganizer.App.Properties;
using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Services;
using ENBOrganizer.Util;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;

namespace ENBOrganizer.App.ViewModels
{
    public abstract class FileSystemViewModel<TEntity> : PageViewModelBase<TEntity> where TEntity : FileSystemEntity
    {
        protected new FileSystemService<TEntity> DataService { get { return (FileSystemService<TEntity>)base.DataService; } }

        public ICommand ViewFilesCommand { get; set; }
        public ICommand ChangeStateCommand { get; set; }
        public ICommand DisableAllCommand { get; set; }
        public ICommand RenameCommand { get; set; }

        public FileSystemViewModel(DataService<TEntity> dataService) : base(dataService)
        {
            ViewFilesCommand = new RelayCommand<TEntity>(entity => Process.Start(entity.Directory.FullName));
            ChangeStateCommand = new RelayCommand<TEntity>(OnStateChanged);
            DisableAllCommand = new RelayCommand(DisableAll);
            RenameCommand = new RelayCommand<Binary>(Rename);

            Settings.Default.PropertyChanged += Default_PropertyChanged;
        }

        private void OnStateChanged(TEntity entity)
        {
            try
            {
                if (entity.IsEnabled)
                    DataService.Disable(entity);
                else
                    DataService.Enable(entity);
            }
            catch (Exception exception)
            {
                _dialogService.ShowErrorDialog(exception.Message);
            }
        }

        private void DisableAll()
        {
            try
            {
                DataService.DisableAll(CurrentGame);
            }
            catch (Exception exception)
            {
                _dialogService.ShowErrorDialog(exception.Message);
            }
        }

        private async void Rename(Binary binary)
        {
            string newName = (string)await _dialogService.ShowInputDialog("Name", "RenameBinaryDialog");

            binary.Name = newName;
            DataService.SaveChanges();
        }

        private void Default_PropertyChanged(object sender, PropertyChangedEventArgs eventArgs)
        {
            if (eventArgs.PropertyName == "CurrentGame")
                PopulateModels();
        }

        protected override void PopulateModels()
        {
            Models.Clear();
            Models.AddAll(DataService.GetAll().Where(binary => binary.Game.Equals(CurrentGame)));
        }
    }
}