using ENBOrganizer.Domain.Entities;
using System.Collections.ObjectModel;
using System.Data.Entity;

namespace ENBOrganizer.Domain.Services
{
    public abstract class DataService<TContext, TEntity> where TEntity : EntityBase where TContext : DbContext, new()
    {
        protected readonly TContext _context;
        public ObservableCollection<TEntity> Items { get { return _context.Set<TEntity>().Local; } }

        public DataService()
        {
            _context = new TContext();
        }
        
        public virtual void Add(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
            _context.SaveChanges();
        }

        public virtual void Delete(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
            _context.SaveChanges();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
