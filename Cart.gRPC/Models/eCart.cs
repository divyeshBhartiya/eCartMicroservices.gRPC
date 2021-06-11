using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cart.gRPC.Models
{
    public class eCart
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public List<CartItem> Items { get; set; } = new List<CartItem>();

        public eCart() { }

        public eCart(string userName)
        {
            UserName = userName;
        }

        public float TotalPrice
        {
            get
            {
                float totalprice = 0;
                foreach (var item in Items)
                {
                    totalprice += item.Price * item.Quantity;
                }
                return totalprice;
            }
        }
    }
}
