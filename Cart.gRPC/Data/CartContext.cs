﻿using Cart.gRPC.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cart.gRPC.Data
{
    public class CartContext : DbContext
    {
        public CartContext(DbContextOptions<CartContext> options) : base(options) { }

        public DbSet<eCart> ShoppingCart { get; set; }
        public DbSet<CartItem> ShoppingCartItem { get; set; }
    }
}
