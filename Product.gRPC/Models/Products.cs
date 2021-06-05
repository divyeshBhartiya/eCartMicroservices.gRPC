using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Product.gRPC.Models
{
    public class Products
    {
        [Key]
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public ProductStatus Status { get; set; }
        public DateTime CreatedTime { get; set; }
    }

    public enum ProductStatus
    {
        INSTOCK = 0,
        LOW = 1,
        NONE = 2
    }
}
