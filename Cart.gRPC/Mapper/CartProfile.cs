using AutoMapper;
using Cart.gRPC.Models;
using Cart.gRPC.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cart.gRPC.Mapper
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<eCart, CartModel>().ReverseMap();
            CreateMap<CartItem, CartItemModel>().ReverseMap();
        }
    }
}
