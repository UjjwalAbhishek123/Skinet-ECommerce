using Core.Entities;
using Infrastructure.Config;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    //StoreContext will represent the database
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions options) : base(options)
        {
        }

        //Define the entites here
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            /*
             OnModelCreating method me ApplyConfigurationsFromAssembly 
            ka use karke sabhi configuration classes ko dynamically apply kiya jata hai.

            ApplyConfigurationsFromAssembly: Jo bhi IEntityTypeConfiguration<T> 
            implement karte hain, unhe automatically context ke saath register karta hai.
             */

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductConfiguration).Assembly);
        }
    }
}
