using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        //injecting IProductRepository object, as we are using here Repository Pattern
        private readonly IProductRepository _repo;

        public ProductsController(IProductRepository repo)
        {
            _repo = repo;
        }

        //creating http endpoints
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand, string? type, string? sort)
        {
            //logic to return list of products, getting it from DB
            return Ok(await _repo.GetProductAsync(brand, type, sort));
        }

        //endpoint for getting product at particular id
        [HttpGet("{id:int}")] //api/products/2
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            //finding product at particular id in Products table
            //product can be null here
            var product = await _repo.GetProductByIdAsync(id);

            //checking if product is null, i.e. product is not found at id
            if(product == null)
            {
                return NotFound();
            }

            return product;
        }

        //endpoint to create product
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            _repo.AddProduct(product);

            if (await _repo.SaveChangesAsync())
            {
                return CreatedAtAction("GetProductById", new { id = product.Id }, product);
            }

            return BadRequest("Problem creating product");
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id, Product product)
        {
            if (product.Id != id || !ProductExist(id))
            {
                return BadRequest("Cannot update this product.");
            }

            //it will tell EF tracker, that what we're passing here is entity effectively and it has been modified
            //_storeContext.Entry(product).State = EntityState.Modified;

            _repo.UpdateProduct(product);

            if (await _repo.SaveChangesAsync())
            {
                return NoContent();
            }

            return BadRequest("Problem updating product");
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _repo.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            _repo.DeleteProduct(product);

            if (await _repo.SaveChangesAsync())
            {
                return NoContent();
            }

            return BadRequest("Problem deleting product");
        }

        //action method to get brands
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
        {
            return Ok(await _repo.GetBrandAsync());
        }

        //action method to get Types
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
        {
            return Ok(await _repo.GetTypesAsync());
        }

        //method to check if product exists or not
        private bool ProductExist(int id)
        {
            return _repo.ProductExists(id);
        }
    }
}
