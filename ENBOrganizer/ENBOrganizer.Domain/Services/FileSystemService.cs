using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Exceptions;
using ENBOrganizer.Util.IO;
using System;
using System.IO;
using System.IO.Compression;

namespace ENBOrganizer.Domain.Services
{
    public class FileSystemService<TEntity> : DataService<TEntity> where TEntity : FileSystemEntity
    {
        private readonly MasterListService _masterListService;

        public FileSystemService(MasterListService masterListService)
        {
            _masterListService = masterListService;
        }

        public void Import(TEntity entity, string sourcePath)
        {
            try
            {
                Add(entity);

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
                Delete(entity);

                throw;
            }
        }

        public new void Delete(TEntity entity)
        {
            entity.Directory.Delete(true);

            base.Delete(entity);
        }
    }
}
