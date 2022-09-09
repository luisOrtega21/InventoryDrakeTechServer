using AutoMapper;
using INVENTORY.SERVER.Common;
using INVENTORY.SERVER.Data.Interfaces;
using INVENTORY.SHARED.Dto;
using INVENTORY.SHARED.Model;
using INVENTORY.SERVER.Services.Interfaces;

namespace INVENTORY.SERVER.Services
{
    public class ProductService : BaseService, IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IMapper mapper, IProductRepository productRepository, ILoggerManager loggerManager)
            : base(mapper, loggerManager)
        {
            _productRepository = productRepository;
        }
        public async Task<ServiceResponse<List<ProductDto>>> GetProducts()
        {
            try
            {
                var products = await _productRepository.GetProducts();
                if (products == null)
                    products = new List<Product>();
                return new ServiceResponse<List<ProductDto>>(Mapper.Map<List<ProductDto>>(products));
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                return new ServiceResponse<List<ProductDto>>($"Error ProductService::GetProducts::{ex.Message}");
            }
        }
        public async Task<ServiceResponse<ProductDto>> GetProductById(Guid id)
        {
            try
            {
                var product = await _productRepository.GetProductById(id);
                if (product == null)
                    throw new ArgumentException("No hay un producto con este identificador");
                return new ServiceResponse<ProductDto>(Mapper.Map<ProductDto>(product));
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                return new ServiceResponse<ProductDto>($"{ex.Message}");
            }
        }
        public async Task<ServiceResponse<ProductDto>> CreateProduct(ProductCreateDto productCreateDto)
        {
            try
            {
                INVENTORY.SHARED.Model.Product product = Mapper.Map<Product>(productCreateDto);
                if (product.Name == null || product.Reference == null)
                    throw new ArgumentException("Una propiedad es requerida");
                product.DateCreated = DateTime.Now;
                
                _productRepository.InventoryDBContext.BeginTransaction();
                _productRepository.Create(product);
                await _productRepository.InventoryDBContext.CommitAsync();

                return new ServiceResponse<ProductDto>(Mapper.Map<ProductDto>(product));
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                return new ServiceResponse<ProductDto>($"{ex.Message}");
            }
        }
        public async Task<ServiceResponse<ProductDto>> UpdateProduct(ProductDto productDto)
        {
            try
            {
                if (productDto == null || productDto.Reference == null || productDto.Name == null)
                    throw new ArgumentException("Error al actualizar un objetivo con este identificador");
                INVENTORY.SHARED.Model.Product productEntity = Mapper.Map<Product>(productDto);
                _productRepository.InventoryDBContext.BeginTransaction();
                var entity = _productRepository.GetSingle(productDto.Id);

                if (entity == null)
                    throw new ArgumentException("No existe un objetivo con este identificador");

                entity.Reference = productEntity.Reference;
                entity.Name = productEntity.Name;
                entity.DateUpdated = productEntity.DateUpdated;
                entity.Date = productEntity.Date;
                entity.Description = productEntity.Description;
                entity.Price = productEntity.Price;
                entity.Quantity = productEntity.Quantity;
                
                _productRepository.Update(entity);
                await _productRepository.InventoryDBContext.CommitAsync();
                return new ServiceResponse<ProductDto>(Mapper.Map<ProductDto>(entity));
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                return new ServiceResponse<ProductDto>($"{ex.Message}");
            }
        }
        public async Task<ServiceResponse<List<ProductDto>>> DeleteProduct (IEnumerable<Guid> ids)
        {
            try
            {
                _productRepository.InventoryDBContext.BeginTransaction();
                List<ProductDto> productDeleted = new List<ProductDto>();
                foreach (var thirdPartyId in ids)
                {
                    var product = _productRepository.GetSingle(thirdPartyId);

                    if (product != null)
                    {
                        _productRepository.Delete(product);
                        productDeleted.Add(Mapper.Map<ProductDto>(product));
                    }
                }

                await _productRepository.InventoryDBContext.CommitAsync();
                return new ServiceResponse<List<ProductDto>>(productDeleted);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                return new ServiceResponse<List<ProductDto>>($"{ex.Message}");
            }
        }
    }
    
}

