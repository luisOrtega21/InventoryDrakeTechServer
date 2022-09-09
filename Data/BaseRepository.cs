using INVENTORY.SERVER.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace INVENTORY.SERVER.Data
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity>
       where TEntity : class
    {
        public InventoryDBContext InventoryDBContext { get; }

        public BaseRepository(InventoryDBContext inventoryDBContext)
        {
            this.InventoryDBContext = inventoryDBContext;
        }

        public virtual TEntity GetSingle(object key)
        {
            return InventoryDBContext.Set<TEntity>().Find(key);
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return InventoryDBContext.Set<TEntity>().ToList();
        }

        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression)
        {
            return InventoryDBContext.Set<TEntity>().Where(expression).ToList();
        }

        public virtual void Update(TEntity entity)
        {
            InventoryDBContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Create(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            InventoryDBContext.Set<TEntity>().Add(entity);
        }

        public virtual int Delete(TEntity entity)
        {
            InventoryDBContext.Set<TEntity>().Remove(entity);
            return 0;
        }
    }
}
