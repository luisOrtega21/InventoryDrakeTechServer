using INVENTORY.SERVER.Extensions;
using INVENTORY.SERVER.Services.Interfaces;
using INVENTORY.SHARED.Dto;
using Microsoft.AspNetCore.Mvc;

namespace GoIn.Backend
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var result = await _productService.GetProducts();

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Entity);
        }

        // GET: api/Product/id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var result = await _productService.GetProductById(id);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Entity);
        }

        // POST: api/Product
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateDto product)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var result = await _productService.CreateProduct(product);

            if (!result.Success)
                return BadRequest(result.Message);

            return CreatedAtAction("GetProductById", new { id = result.Entity.Id }, result);
        }

        // PUT: api/Product/id
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, ProductDto product)
        {
            if (product == null || !ModelState.IsValid || id != product.Id || id == Guid.Empty)
                return BadRequest(ModelState.GetErrorMessages());

            var result = await _productService.UpdateProduct(product);

            if (!result.Success)
                return BadRequest(result.Message);

            return NoContent();
        }

        // DELETE: api/Product/id
        [HttpDelete("{ids}")]
        public async Task<IActionResult> DeleteClientAsync(string ids)
        {
            var idsToDelete = new List<Guid>();
            ids.Split(",").ToList().ForEach(x => { idsToDelete.Add(new Guid(x)); });

            var result = await _productService.DeleteProduct(idsToDelete);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Entity);
        }
    }
}
