using AutoMapper;
using ProductShop.Dtos.Category;
using ProductShop.Dtos.CategoryProduct;
using ProductShop.Dtos.Product;
using ProductShop.Dtos.User;
using ProductShop.Models;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            //Import
            this.CreateMap<ImportUserDto, User>();
            this.CreateMap<ImportProductDto, Product>();
            this.CreateMap<ImportCategoryDto, Category>();
            this.CreateMap<ImportCategoryProductDto, CategoryProduct>();

            //Export
            this.CreateMap<Product, ExportProductDto>()
                .ForMember(d => d.SellerFullName, mo => mo.MapFrom(s => $"{s.Seller.FirstName} {s.Seller.LastName}"));
        }
    }
}
