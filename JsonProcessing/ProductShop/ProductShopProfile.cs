using AutoMapper;
using ProductShop.Dtos.Category;
using ProductShop.Dtos.CategoryProduct;
using ProductShop.Dtos.Product;
using ProductShop.Dtos.User;
using ProductShop.Models;
using System.Linq;

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

            this.CreateMap<Product, ExportSoldProductDto>()
                .ForMember(d => d.BuyerFirstName, mo => mo.MapFrom(s => s.Buyer.FirstName))
                .ForMember(d => d.BuyerLastName, mo => mo.MapFrom(s => s.Buyer.LastName));

            this.CreateMap<User, ExportBuyerDto>()
                .ForMember(d => d.SoldProducts, mo => mo.MapFrom(s => s.ProductsSold.Where(ps => ps.BuyerId.HasValue)));

            this.CreateMap<Category, ExportCategoryDto>()
                .ForMember(d => d.Name, mo => mo.MapFrom(s => s.Name))
                .ForMember(d => d.ProductsCount, mo => mo.MapFrom(s => s.CategoryProducts.Count))
                .ForMember(d => d.AvgPrice, mo => mo.MapFrom(s => s.CategoryProducts.Average(cp => cp.Product.Price).ToString("f2")))
                .ForMember(d => d.TotalRevenue, mo => mo.MapFrom(s => s.CategoryProducts.Sum(cp => cp.Product.Price).ToString("f2")));

            this.CreateMap<Product, ExportSimpleProductDto>();
            this.CreateMap<User, ExportAllSoldProductsDto>()
                .ForMember(d => d.Products, mo => mo.MapFrom(s => s.ProductsSold.Where(p => p.BuyerId.HasValue)));
            this.CreateMap<User, ExportUserWithSoldProductsDto>()
                .ForMember(d => d.SoldProducts, mo => mo.MapFrom(s => s));
        }
    }
}
