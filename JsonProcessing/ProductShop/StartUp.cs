using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductShop.Common;
using ProductShop.Data;
using ProductShop.Dtos.Category;
using ProductShop.Dtos.CategoryProduct;
using ProductShop.Dtos.Product;
using ProductShop.Dtos.User;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {        
        public static void Main(string[] args)
        {
            Mapper.Initialize(cfg => cfg.AddProfile(typeof(ProductShopProfile)));
            ProductShopContext dbContext = new ProductShopContext();

            //string filePath = PathInitializer.CombineImportPath("users.json", Directory.GetCurrentDirectory());
            //string filePath = PathInitializer.CombineImportPath("products.json", Directory.GetCurrentDirectory());
            //string filePath = PathInitializer.CombineImportPath("categories.json", Directory.GetCurrentDirectory());
            //string filePath = PathInitializer.CombineImportPath("categories-products.json", Directory.GetCurrentDirectory());

            //string inputJson = File.ReadAllText(filePath);

            //string filePath = PathInitializer.CombineExportPath("products-in-range.json", Directory.GetCurrentDirectory());
            //string filePath = PathInitializer.CombineExportPath("user-sold-products.json", Directory.GetCurrentDirectory());
            //string filePath = PathInitializer.CombineExportPath("categories-by-products.json", Directory.GetCurrentDirectory());
            string filePath = PathInitializer.CombineExportPath("users-and-products.json", Directory.GetCurrentDirectory());

            //string result = ImportUsers(dbContext, inputJson);
            //string result = ImportProducts(dbContext, inputJson);
            //string result = ImportCategories(dbContext, inputJson);
            //string result = ImportCategoryProducts(dbContext, inputJson);

            //string result = GetProductsInRange(dbContext);
            //string result = GetSoldProducts(dbContext);
            //string result = GetCategoriesByProductsCount(dbContext);
            string result = GetUsersWithProducts(dbContext);

            //dbContext.Database.EnsureDeleted();
            //dbContext.Database.EnsureCreated();

            //Console.WriteLine(result);

            File.WriteAllText(filePath, result);
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            //ExportUserWithSoldProductsDto[] users = context
            //    .Users.Where(u => u.ProductsSold.Any(ps => ps.BuyerId.HasValue))
            //    .OrderByDescending(u => u.ProductsSold.Count(ps => ps.BuyerId.HasValue))
            //    .ProjectTo<ExportUserWithSoldProductsDto>().ToArray();

            ExportAllUsersDto serDto = new ExportAllUsersDto()
            {
                Users = context.Users
               .Where(u => u.ProductsSold.Any(p => p.BuyerId.HasValue))
               .OrderByDescending(u => u.ProductsSold.Count(p => p.BuyerId.HasValue))
               .Select(u => new ExportUserWithSoldProductsDto()
               {
                   FirstName = u.FirstName,
                   LastName = u.LastName,
                   Age = u.Age,
                   SoldProducts = new ExportAllSoldProductsDto()
                   {
                       Products = u.ProductsSold
                       .Where(p => p.BuyerId.HasValue)
                       .Select(p => new ExportSimpleProductDto()
                       {
                           Name = p.Name,
                           Price = p.Price
                       }).ToArray()
                   }
               }).ToArray()
            };

            JsonSerializerSettings serializerSettings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            string json = JsonConvert.SerializeObject(serDto, Formatting.Indented,serializerSettings);
            return json;
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            ExportCategoryDto[] categories = context
                .Categories.OrderByDescending(c => c.CategoryProducts.Count)
                .ProjectTo<ExportCategoryDto>().ToArray();

            string json = JsonConvert.SerializeObject(categories, Formatting.Indented);
            return json;
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            ExportBuyerDto[] buyers = context
                .Users.Where(u => u.ProductsSold.Any(p => p.BuyerId.HasValue))
                .OrderBy(u => u.LastName).ThenBy(u => u.FirstName)
                .ProjectTo<ExportBuyerDto>().ToArray();

            string json = JsonConvert.SerializeObject(buyers, Formatting.Indented);
            return json;
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            ExportProductDto[] products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .ProjectTo<ExportProductDto>().ToArray();

            string json = JsonConvert.SerializeObject(products, Formatting.Indented);

            return json;
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            ImportCategoryProductDto[] categoryProductDtos = JsonConvert
                .DeserializeObject<ImportCategoryProductDto[]>(inputJson);
            ICollection<CategoryProduct> validCategoryProducts = new List<CategoryProduct>();

            foreach (var catPrDto in categoryProductDtos)
            {
                //No need of validation

                if (!IsValid(catPrDto))
                {
                    continue;
                }

                CategoryProduct categoryProduct = Mapper.Map<CategoryProduct>(catPrDto);
                validCategoryProducts.Add(categoryProduct);
            }

            context.CategoryProducts.AddRange(validCategoryProducts);
            context.SaveChanges();

            return $"Successfully imported {validCategoryProducts.Count}";
        } 

        public static string ImportCategories(ProductShopContext dbContext, string inputJson)
        {
            ImportCategoryDto[] categoryDto = JsonConvert
                .DeserializeObject<ImportCategoryDto[]>(inputJson);

            ICollection<Category> validCatgories = new List<Category>();

            foreach (var catDto in categoryDto)
            {
                if (!IsValid(catDto))
                {
                    continue;
                }

                Category category = Mapper.Map<Category>(catDto);
                validCatgories.Add(category);
            }

            dbContext.Categories.AddRange(validCatgories);
            dbContext.SaveChanges();

            return $"Successfully imported {validCatgories.Count}";
        }

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            ImportProductDto[] productDtos = JsonConvert
                .DeserializeObject<ImportProductDto[]>(inputJson);

            ICollection<Product> validProducts = new List<Product>();

            foreach (ImportProductDto pDto in productDtos)
            {
                if (!IsValid(pDto))
                {
                    continue;
                }

                Product product = Mapper.Map<Product>(pDto);
                validProducts.Add(product);
            }

            context.Products.AddRange(validProducts);
            context.SaveChanges();

            return $"Successfully imported {validProducts.Count}";
        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            ImportUserDto[] userDtos = JsonConvert
                .DeserializeObject<ImportUserDto[]>(inputJson);

            ICollection<User> validUsers = new List<User>();
            foreach (var uDto in userDtos)
            {
                if (!IsValid(uDto))
                {
                    continue;
                }

                User user = Mapper.Map<User>(uDto);
                validUsers.Add(user);
            }
            context.Users.AddRange(validUsers);
            context.SaveChanges();

            return $"Successfully imported {validUsers.Count}";
        }       


        /// <summary>
        /// Executes all validation attributes in a class and returns True or False depending on Validation Result.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static bool IsValid(Object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);

            return isValid;
        }
    }
}