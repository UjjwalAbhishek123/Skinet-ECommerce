using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        //creating static class for Seeding data
        public static async Task SeedAsync(StoreContext context)
        {
            if (!context.Products.Any())
            {
                //Read Json file
                var productsData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/products.json");

                //Deserializing json data ==> C# objects
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                //check for product is null or not
                if (products == null)
                {
                    return;
                }

                //use AddRange to add multiple entities to table
                context.Products.AddRange(products);

                await context.SaveChangesAsync();
            }
        }
    }
}
