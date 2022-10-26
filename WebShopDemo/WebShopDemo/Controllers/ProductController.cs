using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebShopDemo.Core.Contracts;
using WebShopDemo.Core.Data.Models;
using WebShopDemo.Core.Models;

namespace WebShopDemo.Controllers
{
    /// <summary>
    /// Web shop products
    /// </summary>
    public class ProductController : BaseController
    {
        private readonly IProductService productService;

        public ProductController(IProductService _productService)
        {
            this.productService = _productService;
        }

        /// <summary>
        /// List all products
        /// </summary>
        /// <returns></returns>
        
        [AllowAnonymous]       
        public async Task<IActionResult> Index()
        {
            var products = await productService.GetAll();
            ViewData["Title"] = "Products";

            return View(products);
        }

        [HttpGet]
        [Authorize(Roles = $"{RoleConstants.Manager}, {RoleConstants.Administrator}")]
        public IActionResult Add()
        {
            var model = new ProductDto();
            return View("Add", model);
        }

        [HttpPost]
        [Authorize(Roles = $"{RoleConstants.Manager}, {RoleConstants.Administrator}")]
        public async Task<IActionResult> Add(ProductDto model)
        {
            if (!ModelState.IsValid)
            {
                return View("Add", model);
            }

            await productService.Add(model);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(Policy = "CanDeleteProduct")]

        public async Task<IActionResult> Delete(string id)
        {
            Guid idGuid = Guid.Parse(id);

            await productService.Delete(idGuid);
            return RedirectToAction(nameof(Index));
        }
    }
}
