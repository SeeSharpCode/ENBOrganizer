using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Exceptions;
using ENBOrganizer.Util.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace ENBOrganizer.Domain.Services
{
    public class FileSystemService<TEntity> : DataService<TEntity> where TEntity : FileSystemEntity
    {
        protected readonly MasterListService _masterListService;

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

        public override void Delete(TEntity entity)
        {
            entity.Directory.Delete(true);

            base.Delete(entity);
        }

        public void Enable(TEntity entity)
        {
            try
            {
                entity.Directory.CopyTo(entity.Game.DirectoryPath);

                entity.IsEnabled = true;
                SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Disable(TEntity entity)
        {
            try
            {
                foreach (FileSystemInfo fileSystemInfo in entity.Directory.GetFileSystemInfos())
                {
                    string installedPath = Path.Combine(entity.Game.DirectoryPath, fileSystemInfo.Name);

                    if (fileSystemInfo is DirectoryInfo && Directory.Exists(installedPath) && fileSystemInfo.Name != DirectoryNames.Data)
                        Directory.Delete(installedPath, true);
                    else if (File.Exists(installedPath))
                        File.Delete(installedPath);
                }

                entity.IsEnabled = false;
                SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DisableAll(Game currentGame)
        {
            try
            {
                foreach (TEntity entity in GetByGame(currentGame))
                    Disable(entity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<TEntity> GetByGame(Game game)
        {
            return GetAll().Where(entity => entity.Game.Equals(game)).ToList();
        }

        public void DeleteByGame(Game game)
        {
            List<TEntity> entities = GetAll().Where(entity => entity.Game.Equals(game)).ToList();

            foreach (TEntity entity in entities)
                Delete(entity);
        }
    }
}
