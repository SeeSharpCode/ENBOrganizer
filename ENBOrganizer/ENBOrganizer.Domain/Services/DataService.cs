using ENBOrganizer.Domain.Data;
using ENBOrganizer.Domain.Entities;
using System;
using System.Collections.Generic;

namespace ENBOrganizer.Domain.Services
{
    public class DataService<TEntity> where TEntity : EntityBase
    {
        protected readonly Repository<TEntity> _repository;

        public event EventHandler<RepositoryChangedEventArgs> ItemsChanged;

        public DataService()
        {
            _repository = new Repository<TEntity>();
        }

        public virtual List<TEntity> Items { get { return _repository.Items; } }
        
        public virtual void Add(TEntity entity)
        {
            _repository.Add(entity);

            RaiseItemsChanged(new RepositoryChangedEventArgs(RepositoryActionType.Added, entity));
        }

        public virtual void Delete(TEntity entity)
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
