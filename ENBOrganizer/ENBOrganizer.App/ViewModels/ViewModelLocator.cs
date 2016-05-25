﻿using ENBOrganizer.App.Messages;
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
            SimpleIoc.Default.Register<AddGameViewModel>();
            SimpleIoc.Default.Register<GamesViewModel>();
            SimpleIoc.Default.Register<PresetsOverviewViewModel>();
            SimpleIoc.Default.Register<GamesViewModel>();
            SimpleIoc.Default.Register<PageViewModelBase<MasterListItem>>
                (() => new PageViewModelBase<MasterListItem>(SimpleIoc.Default.GetInstance<MasterListService>(), 
                SimpleIoc.Default.GetInstance<DialogService>(), DialogName.AddMasterListItem, "Master List"));
            SimpleIoc.Default.Register<AddMasterListItemViewModel>();
            SimpleIoc.Default.Register<BinariesViewModel>();
            SimpleIoc.Default.Register<AddPresetViewModel>();
            SimpleIoc.Default.Register<AddBinaryViewModel>();
        }

        public PresetsOverviewViewModel PresetsOverviewViewModel { get { return SimpleIoc.Default.GetInstance<PresetsOverviewViewModel>(); } }
        public PageViewModelBase<EntityBase> GamesViewModel { get { return SimpleIoc.Default.GetInstance<GamesViewModel>(); } }
        public MainViewModel MainViewModel { get { return SimpleIoc.Default.GetInstance<MainViewModel>(); } }
        public AddGameViewModel AddGameViewModel { get { return SimpleIoc.Default.GetInstance<AddGameViewModel>(); } }
        public PageViewModelBase<MasterListItem> MasterListViewModel { get { return SimpleIoc.Default.GetInstance<PageViewModelBase<MasterListItem>>(); } }
        public AddMasterListItemViewModel AddMasterListItemViewModel { get { return SimpleIoc.Default.GetInstance<AddMasterListItemViewModel>(); } }
        public BinariesViewModel BinariesViewModel { get { return SimpleIoc.Default.GetInstance<BinariesViewModel>(); } }
        public AddPresetViewModel ImportPresetViewModel { get { return SimpleIoc.Default.GetInstance<AddPresetViewModel>(); } }
        public AddBinaryViewModel AddBinaryViewModel { get { return SimpleIoc.Default.GetInstance<AddBinaryViewModel>(); } }
    }
}