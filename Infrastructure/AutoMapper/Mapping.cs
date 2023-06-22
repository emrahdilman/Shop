using ApplicationCore.Entities.Concrete;
using ApplicationCore.Entities.DTO_s.CategoryDTO;
using ApplicationCore.Entities.DTO_s.ProductDTO;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.AutoMapper
{
    public class Mapping : Profile
    {
        public Mapping() 
        {
            CreateMap<Category, CreateCategoryDTO>().ReverseMap();

            CreateMap<Category, UpdateCategoryDTO>().ReverseMap();
            CreateMap<Category, UpdateCategoryDTO>();


            CreateMap<Product, CreateProductDTO>().ReverseMap();
            CreateMap<Product, UpdateProductDTO>().ReverseMap();
        }
    }
}
