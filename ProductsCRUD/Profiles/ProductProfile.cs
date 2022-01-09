using AutoMapper;
using ProductsCRUD.DomainModels;
using ProductsCRUD.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsCRUD.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductDomainModel, ProductReadDTO>();
            CreateMap<ProductPricesDomainModel, ProductPricesReadDTO>();
            CreateMap<ProductCreateDTO, ProductDomainModel>();
            CreateMap<ProductDomainModel, ProductEditDTO>();
            CreateMap<ProductEditDTO, ProductDomainModel>();
        }
    }
}
