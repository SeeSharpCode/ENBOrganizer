using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Services;
using GalaSoft.MvvmLight.Ioc;

namespace ENBOrganizer.App.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            SimpleIoc.Default.Register<DialogService>();
            SimpleIoc.Default.Register<GameService>();
            SimpleIoc.Default.Register<PresetService>();
            SimpleIoc.Default.Register<MasterListService>();
            SimpleIoc.Default.Register<FileSystemService<Binary>>();

            SimpleIoc.Default.Register<ViewModelLocator>();

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<GameDetailViewModel>();
            SimpleIoc.Default.Register<GamesViewModel>();
            SimpleIoc.Default.Register<PresetsOverviewViewModel>();
            SimpleIoc.Default.Register<GamesViewModel>();
            SimpleIoc.Default.Register<MasterListViewModel>();
            SimpleIoc.Default.Register<MasterListItemDetailViewModel>();
            SimpleIoc.Default.Register<BinariesViewModel>();
            SimpleIoc.Default.Register<AddPresetViewModel>();
            SimpleIoc.Default.Register<AddBinaryViewModel>();
        }

        public PresetsOverviewViewModel PresetsOverviewViewModel { get { return SimpleIoc.Default.GetInstance<PresetsOverviewViewModel>(); } }
        public GamesViewModel GamesViewModel { get { return SimpleIoc.Default.GetInstance<GamesViewModel>(); } }
        public MainViewModel MainViewModel { get { return SimpleIoc.Default.GetInstance<MainViewModel>(); } }
        public GameDetailViewModel GameDetailViewModel { get { return SimpleIoc.Default.GetInstance<GameDetailViewModel>(); } }
        public MasterListViewModel MasterListViewModel { get { return SimpleIoc.Default.GetInstance<MasterListViewModel>(); } }
        public MasterListItemDetailViewModel MasterListItemDetailViewModel { get { return SimpleIoc.Default.GetInstance<MasterListItemDetailViewModel>(); } }
        public BinariesViewModel BinariesViewModel { get { return SimpleIoc.Default.GetInstance<BinariesViewModel>(); } }
        public AddPresetViewModel ImportPresetViewModel { get { return SimpleIoc.Default.GetInstance<AddPresetViewModel>(); } }
        public AddBinaryViewModel AddBinaryViewModel { get { return SimpleIoc.Default.GetInstance<AddBinaryViewModel>(); } }
    }
}