using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    //creating blueprint for Products with some properties
    public class Product : BaseEntity
    {
        //Getting Id from BaseEntity class
        //adding "required" keyword to the sring type properties, as they cannot be null in this case
        public required string Name { get; set; }
        public required string Description { get; set; }
        public decimal Price { get; set; }
        public required string PictureUrl { get; set; }
        public required string Type { get; set; }
        public required string Brand { get; set; }
        public int QuantityInStock { get; set; }
    }
}
