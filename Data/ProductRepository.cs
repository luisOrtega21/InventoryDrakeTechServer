using INVENTORY.SERVER.Data.Interfaces;
using INVENTORY.SHARED.Model;
using Microsoft.EntityFrameworkCore;

namespace INVENTORY.SERVER.Data
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(InventoryDBContext inventoryDBContext)
            : base(inventoryDBContext)
        {
        }
        public async Task<List<Product>> GetProducts()
        {
            return await InventoryDBContext.Products
                    .ToListAsync();
        }
        public async Task<Product> GetProductById(Guid id)
        {
            return await InventoryDBContext.Products
                    .FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}
