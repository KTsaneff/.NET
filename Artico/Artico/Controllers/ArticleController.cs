using Artico.Core.Contracts;
using Artico.Models;
using Microsoft.AspNetCore.Mvc;

namespace Artico.Controllers
{
    public class ArticleController : BaseController
    {
        private readonly IArticleService articleService;

        public ArticleController(IArticleService _articleService)
        {
            this.articleService = _articleService;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var model = await articleService.GetAllAsync();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var model = new CreateArticleViewModel()
            {
                Jobs = await articleService.GetJobAsync(),
                Positions = await articleService.GetPositionAsync()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreateArticleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await articleService.AddArticleAsync(model);
                return RedirectToAction(nameof(All));
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Something went wrong! Try again...");
                return View(model);
            }
        }
    }
}
