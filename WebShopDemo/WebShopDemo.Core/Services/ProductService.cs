using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using WebShopDemo.Core.Contracts;
using WebShopDemo.Core.Data.Common;
using WebShopDemo.Core.Data.Models;
using WebShopDemo.Core.Models;

namespace WebShopDemo.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly IConfiguration config;

        private readonly IRepository repo;

        /// <summary>
        /// IoC 
        /// </summary>
        /// <param name="_config">Application configuration</param>
        public ProductService(IConfiguration _config, IRepository _repo)
        {
            config = _config;
            repo = _repo;
        }

        /// <summary>
        /// Adds new product
        /// </summary>
        /// <param name="productDto">Product model</param>
        /// <returns></returns>
        public async Task Add(ProductDto productDto)
        {
            var product = new Product()
            {
                Name = productDto.Name,
                ImageUrl = productDto.ImageUrl,
                Price = productDto.Price,
                Quantity = productDto.Quantity
            };

            await this.repo.AddAsync(product);
            await this.repo.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var product = await repo.All<Product>().FirstOrDefaultAsync(p => p.Id == id);

            if (product != null)
            {
                product.IsDeleted = true;
                await repo.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Gets all products
        /// </summary>
        /// <returns>List of products</returns>
        public async Task<IEnumerable<ProductDto>> GetAll()
        {
            return await repo.AllReadonly<Product>()
                .Where(p => !p.IsDeleted)
                .Select(p => new ProductDto()
                {
                    Id = p.Id,
                    Name = p.Name,
                    ImageUrl = p.ImageUrl,
                    Price = p.Price,
                    Quantity = p.Quantity
                }).OrderBy(p => p.Name).ToListAsync();
        }
    }
}
