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

        public void DisableAll(Game currentGame)
        {
            foreach (TEntity entity in GetByGame(currentGame))
            {
                entity.Disable();
                entity.IsEnabled = false;
            }

            SaveChanges();
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

        public void Rename(TEntity entity, string newName)
        {
            entity.Directory.Rename(newName);
            entity.Name = newName;

            SaveChanges();
        }
    }
}
