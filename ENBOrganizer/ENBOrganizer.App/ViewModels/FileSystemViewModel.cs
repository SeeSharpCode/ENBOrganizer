using ENBOrganizer.App.Properties;
using ENBOrganizer.Domain.Data;
using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Services;
using GalaSoft.MvvmLight.CommandWpf;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;

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

        public FileSystemViewModel(DataService<ENBOrganizerContext, TEntity> dataService) : base(dataService)
        {
            ViewFilesCommand = new RelayCommand<TEntity>(entity => Process.Start(entity.Directory.FullName));
            ChangeStateCommand = new RelayCommand<TEntity>(OnStateChanged);
            DisableAllCommand = new RelayCommand(DisableAll, CanDisableAll);
            EditCommand = new RelayCommand<TEntity>(Edit);

            Settings.Default.PropertyChanged += Default_PropertyChanged;

            Models = CurrentGame.Presets.;
        }

        private bool CanDisableAll()
        {
            return Models.Any();
        }

        protected abstract void Edit(TEntity entity);

        protected override bool CanAdd()
        {
            return CurrentGame != null;
        }

        private void OnStateChanged(TEntity entity)
        {
            entity.ChangeState();
            DataService.SaveChanges();
        }

        private void DisableAll()
        {
            DataService.DisableAll(CurrentGame);
        }

        private void Default_PropertyChanged(object sender, PropertyChangedEventArgs eventArgs)
        {
            //if (eventArgs.PropertyName == nameof(CurrentGame))
            //    PopulateModels();
        }
    }
}