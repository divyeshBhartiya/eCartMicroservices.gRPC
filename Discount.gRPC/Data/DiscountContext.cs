using Discount.gRPC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discount.gRPC.Data
{
    public static class DiscountContext
    {
        public static readonly List<Offer> Discounts = new List<Offer>
        {
            new Offer{DiscountId = 1, Code = "CODE_100", Amount = 100 },
            new Offer{DiscountId = 1, Code = "CODE_200", Amount = 200 },
            new Offer{DiscountId = 1, Code = "CODE_300", Amount = 300 }
        };
    }
}
