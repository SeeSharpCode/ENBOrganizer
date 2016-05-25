using ENBOrganizer.App.Messages;
using ENBOrganizer.App.Properties;
using ENBOrganizer.Domain;
using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Services;
using ENBOrganizer.Util;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;

namespace ENBOrganizer.App.ViewModels
{
    public class BinariesViewModel : PageViewModelBase<Binary>
    {
        protected readonly new FileSystemService<Binary> _dataService;

        public ICommand ViewFilesCommand { get; set; }
        public ICommand ChangeBinaryStateCommand { get; set; }
        public ICommand DisableAllCommand { get; set; }

        public BinariesViewModel(FileSystemService<Binary> binaryService, DialogService dialogService)
            : base(binaryService, dialogService, DialogName.AddBinary, "Binaries")
        {
            ViewFilesCommand = new RelayCommand<Binary>(binary => Process.Start(binary.Directory.FullName));
            ChangeBinaryStateCommand = new RelayCommand<Binary>(OnBinaryStateChanged);
            DisableAllCommand = new RelayCommand(DisableAll);
        }

        private void DisableAll()
        {
            try
            {
                _dataService.DisableAll(CurrentGame);
            }
            catch (Exception exception)
            {
                _dialogService.ShowErrorDialog(exception.Message);
            }
        }

        // TODO: this code is repeated
        protected new void PopulateModels()
        {
            Models = _dataService.GetAll().Where(binary => binary.Game.Equals(CurrentGame)).ToObservableCollection();
        }

        private void OnBinaryStateChanged(Binary binary)
        {
            try
            {
                if (binary.IsEnabled)
                    _dataService.Disable(binary);
                else
                    _dataService.Enable(binary);
            }
            catch (Exception exception)
            {
                _dialogService.ShowErrorDialog(exception.Message);
            }
        }
    }
}