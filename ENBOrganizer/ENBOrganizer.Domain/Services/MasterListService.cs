using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Exceptions;
using ENBOrganizer.Util;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ENBOrganizer.Domain.Services
{
    public class MasterListService : DataService<MasterListItem>
    {
        private static List<MasterListItem> _defaultMasterListItems
        {
            get
            {
                return new List<MasterListItem>
                {
                    new MasterListItem("enbseries", MasterListItemType.Directory),
                    new MasterListItem("injFX_Shaders", MasterListItemType.Directory),
                    new MasterListItem("SweetFX", MasterListItemType.Directory),
                    new MasterListItem("d3d9_smaa.dll", MasterListItemType.File),
                    new MasterListItem("d3d9.fx", MasterListItemType.File),
                    new MasterListItem("d3d9injFX.dll", MasterListItemType.File),
                    new MasterListItem("d3d9SFX.dll", MasterListItemType.File),
                    new MasterListItem("dxgi.dll", MasterListItemType.File),
                    new MasterListItem("dxgi.fx", MasterListItemType.File),
                    new MasterListItem("effect.txt", MasterListItemType.File),
                    new MasterListItem("enbbloom.fx", MasterListItemType.File),
                    new MasterListItem("enbeffect.fx", MasterListItemType.File),
                    new MasterListItem("enbeffectprepass.fx", MasterListItemType.File),
                    new MasterListItem("enblens.fx", MasterListItemType.File),
                    new MasterListItem("enblensmask.png", MasterListItemType.File),
                    new MasterListItem("enblocal.ini", MasterListItemType.File),
                    new MasterListItem("enbpalette.bmp", MasterListItemType.File),
                    new MasterListItem("enbraindrops.tga", MasterListItemType.File),
                    new MasterListItem("enbraindrops_small.tga", MasterListItemType.File),
                    new MasterListItem("enbseries.ini", MasterListItemType.File),
                    new MasterListItem("enbsunsprite.fx", MasterListItemType.File),
                    new MasterListItem("enbsunsprite.tga", MasterListItemType.File),
                    new MasterListItem("FXAA_Tool.exe", MasterListItemType.File),
                    new MasterListItem("injector.ini", MasterListItemType.File),
                    new MasterListItem("shader.fx", MasterListItemType.File),
                    new MasterListItem("SMAA.fx", MasterListItemType.File),
                    new MasterListItem("SMAA.h", MasterListItemType.File),
                    new MasterListItem("SweetFX readme.txt", MasterListItemType.File),
                    new MasterListItem("SweetFX_preset.txt", MasterListItemType.File),
                    new MasterListItem("SweetFX_settings.txt", MasterListItemType.File),
                    new MasterListItem("d3d11.dll", MasterListItemType.File),
                    new MasterListItem("d3dcompiler_46e.dll", MasterListItemType.File),
                    new MasterListItem("enbadaptation.fx", MasterListItemType.File),
                    new MasterListItem("enbdepthoffield.fx", MasterListItemType.File),
                    new MasterListItem("enbeffectpostpass.fx", MasterListItemType.File),
                };
            }
        }

        public override void Add(MasterListItem masterListItem)
        {
            if (DirectoryNames.EssentialNames.Any(name => name.EqualsIgnoreCase(masterListItem.Name)))
                return;

            base.Add(masterListItem);
        }

        public void CreateMasterListItems(DirectoryInfo directory)
        {
            foreach (FileSystemInfo fileSystemInfo in directory.GetFileSystemInfos())
            {
                MasterListItemType masterListItemType = fileSystemInfo is DirectoryInfo ? MasterListItemType.Directory : MasterListItemType.File;

                MasterListItem masterListItem = new MasterListItem(fileSystemInfo.Name, masterListItemType);

                try
                {
                    Add(masterListItem);
                }
                catch (DuplicateEntityException) { }
            }
        }

        public void AddDefaultItems()
        {
            foreach (MasterListItem masterListItem in _defaultMasterListItems)
                Add(masterListItem);
        }
    }
}
