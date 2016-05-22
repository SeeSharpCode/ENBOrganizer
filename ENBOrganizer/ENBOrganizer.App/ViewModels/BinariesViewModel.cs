using ENBOrganizer.App.Messages;
using ENBOrganizer.Domain;
using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Services;
using ENBOrganizer.Util;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace ENBOrganizer.App.ViewModels
{
    public class BinariesViewModel : ViewModelBase, IPageViewModel
    {
        private readonly BinaryService _binaryService;
        private readonly DialogService _dialogService;

        public string Name { get { return "Binaries"; } }
        public ICommand OpenAddBinaryDialogCommand { get; set; }
        public ICommand OpenFilesCommand { get; set; }
        public ICommand DeleteBinaryCommand { get; set; }
        public ObservableCollection<Binary> Binaries { get; set; }

        private bool _isAddBinaryDialogOpen;

        public bool IsAddBinaryDialogOpen
        {
            get { return _isAddBinaryDialogOpen; }
            set { Set(nameof(IsAddBinaryDialogOpen), ref _isAddBinaryDialogOpen, value); }
        }
        
        public BinariesViewModel(BinaryService binaryService, DialogService dialogService)
        { 
            _binaryService = binaryService;
            _binaryService.ItemsChanged += _binaryService_ItemsChanged;

            _dialogService = dialogService;
            
            OpenFilesCommand = new RelayCommand<Binary>(binary => Process.Start(binary.Directory.FullName));
            DeleteBinaryCommand = new RelayCommand<Binary>(binary => _binaryService.Delete(binary));
            OpenAddBinaryDialogCommand = new RelayCommand(() => _dialogService.ShowDialog(DialogName.AddBinary));

            MessengerInstance.Register<DialogMessage>(this, OnDialogMessageReceived);

            Binaries = _binaryService.GetAll().ToObservableCollection();
        }

        private void OnDialogMessageReceived(DialogMessage message)
        {
            if (message.DialogName != DialogName.AddBinary)
                return;

            IsAddBinaryDialogOpen = message.DialogAction == DialogAction.Open;
        }

        private void _binaryService_ItemsChanged(object sender, RepositoryChangedEventArgs repositoryChangedEventArgs)
        {
            if (repositoryChangedEventArgs.RepositoryActionType == RepositoryActionType.Added)
                Binaries.Add(repositoryChangedEventArgs.Entity as Binary);
            else
                Binaries.Remove(repositoryChangedEventArgs.Entity as Binary);
        }
    }
}