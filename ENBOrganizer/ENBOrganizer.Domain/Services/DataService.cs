using ENBOrganizer.Domain.Data;
using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Exceptions;
using System;
using System.Collections.Generic;

namespace ENBOrganizer.Domain.Services
{
    public class DataService<TEntity> where TEntity : IEntity
    {
        protected readonly Repository<TEntity> _repository;

        public event EventHandler<RepositoryChangedEventArgs> ItemsChanged;

        public DataService(string repositoryName)
        {
            _repository = new Repository<TEntity>(repositoryName);
        }

        public DataService() : this(typeof(TEntity).Name + "s.xml") { }

        public List<TEntity> GetAll()
        {
            return _repository.Items;
        }

        /// <exception cref="DuplicateEntityException" />
        public void Add(TEntity entity)
        {
            try
            {
                _repository.Add(entity);

                RaiseItemsChanged(new RepositoryChangedEventArgs(RepositoryActionType.Added, entity));
            }
            catch (DuplicateEntityException)
            {
                throw;
            }
        }

        public void Delete(TEntity entity)
        {
            _repository.Delete(entity);

            RaiseItemsChanged(new RepositoryChangedEventArgs(RepositoryActionType.Deleted, entity));
        }

        public void SaveChanges()
        {
            _repository.SaveChanges();
        }

        public void RaiseItemsChanged(RepositoryChangedEventArgs eventArgs)
        {
            ItemsChanged?.Invoke(this, eventArgs);
        }
    }
}
