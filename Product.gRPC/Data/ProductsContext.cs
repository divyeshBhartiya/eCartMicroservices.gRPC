using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Product.gRPC.Models;

namespace Product.gRPC.Data
{
    public class ProductsContext : DbContext
    {
        public ProductsContext(DbContextOptions<ProductsContext> options) : base(options) { }
        public DbSet<Products> Product { get; set; }
    }
}
