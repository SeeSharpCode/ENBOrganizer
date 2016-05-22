using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Util.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENBOrganizer.Domain.Services
{
    public class FileSystemService
    {
        private readonly DataService<FileSystemEntity> _dataService;
        private readonly MasterListService _masterListService;

        public FileSystemService(DataService<FileSystemEntity> dataService, MasterListService masterListService)
        {
            _dataService = dataService;
            _masterListService = masterListService;
        }

        public void Import(FileSystemEntity entity, string sourcePath)
        {
            try
            {
                _dataService.Add(entity);

                DirectoryInfo sourceDirectory = new DirectoryInfo(sourcePath);

                if (sourceDirectory.Exists)
                    sourceDirectory.CopyTo(entity.Directory.FullName);
                else
                    ZipFile.ExtractToDirectory(sourcePath, entity.Directory.FullName);

                _masterListService.CreateMasterListItems(entity.Directory);
            }
            catch (DuplicateEntityException)
            {
                throw;
            }
            catch (Exception)
            {
                Delete(binary);

                throw;
            }
        }
    }
}
