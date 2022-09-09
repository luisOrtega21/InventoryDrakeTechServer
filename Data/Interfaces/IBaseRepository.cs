using System.Linq.Expressions;

namespace INVENTORY.SERVER.Data.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        InventoryDBContext InventoryDBContext { get; }
        TEntity GetSingle(object key);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression);
        void Update(TEntity entity);
        void Create(TEntity entity);
        int Delete(TEntity entity);
    }
}

