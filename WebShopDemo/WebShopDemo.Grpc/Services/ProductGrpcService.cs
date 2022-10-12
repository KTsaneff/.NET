using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using WebShopDemo.Core.Contracts;

namespace WebShopDemo.Grpc.Services
{
    public class ProductGrpcService : product.productBase
    {
        private readonly IProductService productService;

        public ProductGrpcService(IProductService _productService)
        {
            this.productService = _productService;
        }

        public override async Task<ProductList> GetAll(Empty request, ServerCallContext context)
        {
            ProductList result = new ProductList();
            var products = await productService.GetAll();

            result.Items.AddRange(products.Select(p => new ProductItem()
            {
                Name = p.Name,
                Id = p.Id.ToString(),
                ImageUrl = p.ImageUrl,
                Price = (double)p.Price,
                Quantity = p.Quantity
            }));

            return result;
        }
    }
}
