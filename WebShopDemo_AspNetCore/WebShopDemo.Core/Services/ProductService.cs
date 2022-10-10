using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text.Json.Nodes;
using WebShopDemo.Core.Contracts;
using WebShopDemo.Core.Data.Common;
using WebShopDemo.Core.Data.Models;
using WebShopDemo.Core.Models;

namespace WebShopDemo.Core.Services
{
    /// <summary>
    /// Manipulates product data
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly IConfiguration config;
        private readonly IRepository repo;

        /// <summary>
        /// IoC 
        /// </summary>
        /// <param name="_config">Application configuration</param>
        public ProductService(IConfiguration _config,
                              IRepository _repo)
        {
            config = _config;
            repo = _repo;
        }

        /// <summary>
        /// Add new product
        /// </summary>
        /// <param name="productDto">Product model</param>
        /// <returns></returns>
        public async Task Add(ProductDto productDto)
        {
            var product = new Product()
            {
                Name = productDto.Name,
                Price = productDto.Price,
                Quantity = productDto.Quantity,
            };

            await repo.AddAsync(product);
            await repo.SaveChangesAsync();
        }

        public async Task Delete(string id)
        {
            var product = await repo.All<Product>()
                .FirstOrDefaultAsync(p => p.Id.ToString() == id);

            if (product != null)
            {
                product.IsActive = false;
                await repo.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Gets all productc
        /// </summary>
        /// <returns>List of products</returns>
        public async Task<IEnumerable<ProductDto>> GetAll()
        {
            return await repo.AllReadonly<Product>()
                .Where(p => p.IsActive)
                .Select(p => new ProductDto()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price= p.Price,
                    Quantity= p.Quantity
                })
                .ToListAsync();
        }
    }
}
