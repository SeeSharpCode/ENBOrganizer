using ENBOrganizer.App.ViewModels.Binaries;
using ENBOrganizer.App.ViewModels.Games;
using ENBOrganizer.App.ViewModels.Master;
using ENBOrganizer.App.ViewModels.Presets;
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
            SimpleIoc.Default.Register<SettingsService>();

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<GameDetailViewModel>();
            SimpleIoc.Default.Register<GamesViewModel>();
            SimpleIoc.Default.Register<PresetsViewModel>();
            SimpleIoc.Default.Register<GamesViewModel>();
            SimpleIoc.Default.Register<MasterListViewModel>();
            SimpleIoc.Default.Register<AddMasterListItemViewModel>();
            SimpleIoc.Default.Register<BinariesViewModel>();
            SimpleIoc.Default.Register<AddPresetViewModel>();
            SimpleIoc.Default.Register<EditPresetViewModel>();
            SimpleIoc.Default.Register<AddBinaryViewModel>();
            SimpleIoc.Default.Register<InputViewModel>();
        }

        public PresetsViewModel PresetsViewModel { get { return SimpleIoc.Default.GetInstance<PresetsViewModel>(); } }
        public GamesViewModel GamesViewModel { get { return SimpleIoc.Default.GetInstance<GamesViewModel>(); } }
        public MainViewModel MainViewModel { get { return SimpleIoc.Default.GetInstance<MainViewModel>(); } }
        public GameDetailViewModel GameDetailViewModel { get { return SimpleIoc.Default.GetInstance<GameDetailViewModel>(); } }
        public MasterListViewModel MasterListViewModel { get { return SimpleIoc.Default.GetInstance<MasterListViewModel>(); } }
        public AddMasterListItemViewModel AddMasterListItemViewModel { get { return SimpleIoc.Default.GetInstance<AddMasterListItemViewModel>(); } }
        public BinariesViewModel BinariesViewModel { get { return SimpleIoc.Default.GetInstance<BinariesViewModel>(); } }
        public AddPresetViewModel AddPresetViewModel { get { return SimpleIoc.Default.GetInstance<AddPresetViewModel>(); } }
        public EditPresetViewModel EditPresetViewModel { get { return SimpleIoc.Default.GetInstance<EditPresetViewModel>(); } }
        public AddBinaryViewModel AddBinaryViewModel { get { return SimpleIoc.Default.GetInstance<AddBinaryViewModel>(); } }
        public InputViewModel InputViewModel { get { return SimpleIoc.Default.GetInstance<InputViewModel>(); } }
    }
}