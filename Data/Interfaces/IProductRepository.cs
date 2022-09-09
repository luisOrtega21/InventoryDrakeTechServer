using INVENTORY.SHARED.Model;

namespace INVENTORY.SERVER.Data.Interfaces
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        Task<List<Product>> GetProducts();
        Task<Product> GetProductById(Guid id);
    }
}

