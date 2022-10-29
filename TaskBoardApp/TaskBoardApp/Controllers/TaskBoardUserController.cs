using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskBoardApp.Data.Entities;
using TaskBoardApp.Models;

namespace TaskBoardApp.Controllers
{
    public class TaskBoardUserController : BaseController
    {
        private readonly UserManager<TaskBoardUser> userManager;
        private readonly SignInManager<TaskBoardUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public TaskBoardUserController(
            UserManager<TaskBoardUser> _userManager, 
            SignInManager<TaskBoardUser> _signInManager,
            RoleManager<IdentityRole> _roleManager)
        {
            this.userManager = _userManager;
            this.signInManager = _signInManager;
            this.roleManager = _roleManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            var model = new RegisterViewModel();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new TaskBoardUser()
            {
                UserName = model.Username,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("Login", "TaskBoardUser");
            }

            foreach (var ex in result.Errors)
            {
                ModelState.AddModelError("", ex.Description);
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            var model = new LoginViewModel()
            {
                ReturnUrl = returnUrl
            };

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.FindByNameAsync(model.Username);

            if (user != null)
            {
                var result = await signInManager.PasswordSignInAsync(user, model.Password, false, false);
                if (result.Succeeded)
                {
                    if (model.ReturnUrl != null)
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError("", "Invalid Login!");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
