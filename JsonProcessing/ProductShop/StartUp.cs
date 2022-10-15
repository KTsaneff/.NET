﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
        private static string filePath = "";
        public static void Main(string[] args)
        {
            Mapper.Initialize(cfg => cfg.AddProfile(typeof(ProductShopProfile)));
            ProductShopContext dbContext = new ProductShopContext();

            //InitializeFilePath("users.json");
            //InitializeFilePath("products.json");
            //InitializeFilePath("categories.json");
            InitializeFilePath("categories-products.json");


            string inputJson = File.ReadAllText(filePath);

            //string result = ImportUsers(dbContext, inputJson);
            //string result = ImportProducts(dbContext, inputJson);
            //string result = ImportCategories(dbContext, inputJson);
            string result = ImportCategoryProducts(dbContext, inputJson);

            //dbContext.Database.EnsureDeleted();
            //dbContext.Database.EnsureCreated();

            Console.WriteLine(result);
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

        private static void InitializeFilePath(string fileName)
        {
            filePath = Path.Combine(Directory.GetCurrentDirectory(), "../../../Datasets/", fileName);
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