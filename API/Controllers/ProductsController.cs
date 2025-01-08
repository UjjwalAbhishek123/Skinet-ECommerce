using Core.Entities;
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
        //injecting StoreContext object
        private readonly StoreContext _storeContext;

        public ProductsController(StoreContext storeContext)
        {
            _storeContext = storeContext;
        }

        //creating http endpoints
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            //logic to return list of products, getting it from DB
            return await _storeContext.Products.ToListAsync();
        }

        //endpoint for getting product at particular id
        [HttpGet("{id:int}")] //api/products/2
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            //finding product at particular id in Products table
            //product can be null here
            var product = await _storeContext.Products.FindAsync(id);

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
            _storeContext.Products.Add(product);

            await _storeContext.SaveChangesAsync();

            return product;
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id, Product product)
        {
            if (product.Id != id || !ProductExist(id))
            {
                return BadRequest("Cannot update this product.");
            }

            //it will tell EF tracker, that what we're passing here is entity effectively and it has been modified
            _storeContext.Entry(product).State = EntityState.Modified;

            await _storeContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _storeContext.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            _storeContext.Products.Remove(product);
            await _storeContext.SaveChangesAsync();

            return NoContent();
        }

        //method to check if product exists or not
        private bool ProductExist(int id)
        {
            return _storeContext.Products.Any(p => p.Id == id);
        }
    }
}
