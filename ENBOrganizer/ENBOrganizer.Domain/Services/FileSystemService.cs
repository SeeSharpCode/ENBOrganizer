using ENBOrganizer.Domain.Data;
using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Exceptions;
using ENBOrganizer.Util.IO;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace ENBOrganizer.Domain.Services
{
    public class FileSystemService<TEntity> : DataService<ENBOrganizerContext, TEntity> where TEntity : FileSystemEntity
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

        public void DisableAll(Game game)
        {
            foreach (TEntity entity in Items.Where(entity => entity.Game.Equals(game)))
                entity.Disable();
        }
        
        public void DeleteByGame(Game game)
        {
            foreach (TEntity entity in Items.Where(entity => entity.Game.Equals(game)))
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
