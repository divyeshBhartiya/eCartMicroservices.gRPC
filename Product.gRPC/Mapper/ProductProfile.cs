using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Product.gRPC.Models;
using Product.gRPC.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Product.gRPC.Mapper
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Products, ProductModel>()
                .ForMember(dest => dest.CreatedTime, opt => opt.MapFrom(src => Timestamp.FromDateTime(src.CreatedTime)));

            CreateMap<ProductModel, Products>()
                .ForMember(dest => dest.CreatedTime, opt => opt.MapFrom(src => src.CreatedTime.ToDateTime()));

            // note : not use reverseMap. Timestamp should be converted manually.
        }
    }
}
