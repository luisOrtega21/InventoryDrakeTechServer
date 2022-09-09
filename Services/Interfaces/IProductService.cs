using INVENTORY.SERVER.Common;
using INVENTORY.SHARED.Dto;

namespace INVENTORY.SERVER.Services.Interfaces
{
    public interface IProductService
    {
        Task<ServiceResponse<List<ProductDto>>> GetProducts();
        Task<ServiceResponse<ProductDto>> GetProductById(Guid id);
        Task<ServiceResponse<ProductDto>> CreateProduct(ProductCreateDto productCreate);
        Task<ServiceResponse<ProductDto>> UpdateProduct(ProductDto productDto);
        Task<ServiceResponse<List<ProductDto>>> DeleteProduct(IEnumerable<Guid> ids);
    }
}

