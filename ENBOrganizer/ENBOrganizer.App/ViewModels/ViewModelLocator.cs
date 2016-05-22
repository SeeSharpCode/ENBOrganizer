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
            SimpleIoc.Default.Register<BinaryService>();

            SimpleIoc.Default.Register<ViewModelLocator>();

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<AddGameViewModel>();
            SimpleIoc.Default.Register<GamesViewModel>();
            SimpleIoc.Default.Register<PresetsOverviewViewModel>();
            SimpleIoc.Default.Register<GamesViewModel>();
            SimpleIoc.Default.Register<MasterListViewModel>();
            SimpleIoc.Default.Register<AddMasterListItemViewModel>();
            SimpleIoc.Default.Register<BinariesViewModel>();
            SimpleIoc.Default.Register<ImportPresetViewModel>();
            SimpleIoc.Default.Register<AddBinaryViewModel>();
        }

        public PresetsOverviewViewModel PresetsOverviewViewModel { get { return SimpleIoc.Default.GetInstance<PresetsOverviewViewModel>(); } }
        public GamesViewModel GamesViewModel { get { return SimpleIoc.Default.GetInstance<GamesViewModel>(); } }
        public MainViewModel MainViewModel { get { return SimpleIoc.Default.GetInstance<MainViewModel>(); } }
        public AddGameViewModel AddGameViewModel { get { return SimpleIoc.Default.GetInstance<AddGameViewModel>(); } }
        public MasterListViewModel MasterListViewModel { get { return SimpleIoc.Default.GetInstance<MasterListViewModel>(); } }
        public AddMasterListItemViewModel AddMasterListItemViewModel { get { return SimpleIoc.Default.GetInstance<AddMasterListItemViewModel>(); } }
        public BinariesViewModel BinariesViewModel { get { return SimpleIoc.Default.GetInstance<BinariesViewModel>(); } }
        public ImportPresetViewModel ImportPresetViewModel { get { return SimpleIoc.Default.GetInstance<ImportPresetViewModel>(); } }
        public AddBinaryViewModel AddBinaryViewModel { get { return SimpleIoc.Default.GetInstance<AddBinaryViewModel>(); } }
    }
}