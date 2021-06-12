using Cart.gRPC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cart.gRPC.Data
{
    public class CartContextSeed
    {
        public static void SeedAsync(CartContext cartContext)
        {
            if (!cartContext.eCart.Any())
            {
                var carts = new List<eCart>
                {
                    new eCart
                    {
                        UserName = "dvs",
                        Items = new List<CartItem>
                        {
                           new CartItem
                           {
                               Quantity = 2,
                               Color = "Black",
                               Price = 699,
                               ProductId = 1,
                               ProductName = "Mi10T"
                           },
                           new CartItem
                           {
                               Quantity = 3,
                               Color = "Red",
                               Price = 899,
                               ProductId = 2,
                               ProductName = "P40"
                           }
                        }
                    }
                };
                cartContext.eCart.AddRange(carts);
                cartContext.SaveChanges();
            }
        }
    }
}
