using ENBOrganizer.App.Messages;
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
    public class BinariesViewModel : PageViewModelBase<Binary>
    {
        protected new FileSystemService<Binary> DataService { get { return (FileSystemService<Binary>)base.DataService; } }
        protected override DialogName DialogName { get { return DialogName.AddBinary; } }

        public ICommand ViewFilesCommand { get; set; }
        public ICommand ChangeBinaryStateCommand { get; set; }
        public ICommand DisableAllCommand { get; set; }
        
        public override string Name { get { return "Binaries"; } }

        public BinariesViewModel(FileSystemService<Binary> binaryService, DialogService dialogService)
            : base(binaryService, dialogService)
        {
            ViewFilesCommand = new RelayCommand<Binary>(binary => Process.Start(binary.Directory.FullName));
            ChangeBinaryStateCommand = new RelayCommand<Binary>(OnBinaryStateChanged);
            DisableAllCommand = new RelayCommand(DisableAll);

            Settings.Default.PropertyChanged += Default_PropertyChanged;
        }

        private void Default_PropertyChanged(object sender, PropertyChangedEventArgs eventArgs)
        {
            if (eventArgs.PropertyName == "CurrentGame")
                PopulateModels();
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

        // TODO: this code is repeated
        protected override void PopulateModels()
        {
            Models.Clear();
            Models.AddAll(DataService.GetAll().Where(binary => binary.Game.Equals(CurrentGame)));
        }

        private void OnBinaryStateChanged(Binary binary)
        {
            try
            {
                if (binary.IsEnabled)
                    DataService.Disable(binary);
                else
                    DataService.Enable(binary);
            }
            catch (Exception exception)
            {
                _dialogService.ShowErrorDialog(exception.Message);
            }
        }
    }
}