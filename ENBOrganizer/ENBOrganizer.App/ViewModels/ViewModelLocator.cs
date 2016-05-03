using ENBOrganizer.Domain.Services;
using ENBOrganizer.Domain.Entities;
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
            SimpleIoc.Default.Register<DataService<MasterListItem>>();
            SimpleIoc.Default.Register<PresetItemsService>();

            SimpleIoc.Default.Register<ViewModelLocator>();

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<AddGameViewModel>();
            SimpleIoc.Default.Register<GamesViewModel>();
            SimpleIoc.Default.Register<PresetsOverviewViewModel>();
            SimpleIoc.Default.Register<PresetFilesViewModel>();
            SimpleIoc.Default.Register<PresetDetailViewModel>();
            SimpleIoc.Default.Register<GamesViewModel>();
            SimpleIoc.Default.Register<MasterListViewModel>();
            SimpleIoc.Default.Register<AddMasterListItemViewModel>();
        }

        public PresetsOverviewViewModel PresetsOverviewViewModel { get { return SimpleIoc.Default.GetInstance<PresetsOverviewViewModel>(); } }
        public GamesViewModel GamesViewModel { get { return SimpleIoc.Default.GetInstance<GamesViewModel>(); } }
        public MainViewModel MainViewModel { get { return SimpleIoc.Default.GetInstance<MainViewModel>(); } }
        public AddGameViewModel AddGameViewModel { get { return SimpleIoc.Default.GetInstance<AddGameViewModel>(); } }
        public PresetDetailViewModel PresetDetailViewModel { get { return SimpleIoc.Default.GetInstance<PresetDetailViewModel>(); } }
        public PresetFilesViewModel PresetFilesViewModel { get { return SimpleIoc.Default.GetInstance<PresetFilesViewModel>(); } }
        public MasterListViewModel MasterListViewModel { get { return SimpleIoc.Default.GetInstance<MasterListViewModel>(); } }
        public AddMasterListItemViewModel AddMasterListItemViewModel { get { return SimpleIoc.Default.GetInstance<AddMasterListItemViewModel>(); } }
    }
}