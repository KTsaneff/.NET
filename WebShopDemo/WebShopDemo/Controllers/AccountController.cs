using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebShopDemo.Core.Constants;
using WebShopDemo.Core.Contracts;
using WebShopDemo.Core.Data.Models.Account;
using WebShopDemo.Models;

namespace WebShopDemo.Controllers
{
    public class AccountController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(
            UserManager<ApplicationUser> _userManager, 
            SignInManager<ApplicationUser> _signInManager,
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

            var user = new ApplicationUser()
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                EmailConfirmed = true,
                UserName = model.Email
            };

            var result = await userManager.CreateAsync(user, model.Password);
            await userManager
                    .AddClaimAsync(user, new System.Security.Claims.Claim(ClaimTypeConstants.FirstName, user.FirstName ?? user.Email));

            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, isPersistent: false);

                return RedirectToAction("Index", "Home");
            }

            foreach (var item in result.Errors)
            {
                ModelState.AddModelError("", item.Description);
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            var model = new LoginViewModel()
            {
                ReturnUrl = returnUrl,
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

            var user = await userManager.FindByEmailAsync(model.Email);

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

            ModelState.AddModelError("", "Invalid login!");

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> CreateRoles()
        {
            await roleManager.CreateAsync(new IdentityRole(RoleConstants.Manager));
            await roleManager.CreateAsync(new IdentityRole(RoleConstants.Supervisor));
            await roleManager.CreateAsync(new IdentityRole(RoleConstants.Administrator));
            await roleManager.CreateAsync(new IdentityRole(RoleConstants.Guest));

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> AddUsersToRoles()
        {
            string email1 = "krassytsaneff@gmail.com";
            string email2 = "stefko@abv.bg";
            string email3 = "sia@abv.bg";
            string email4 = "alex@abv.bg";
            string email5 = "dima@abv.bg";
            string email6 = "stefi@abv.bg";

            var user1 = await userManager.FindByEmailAsync(email1);
            var user2 = await userManager.FindByEmailAsync(email2);
            var user3 = await userManager.FindByEmailAsync(email3);
            var user4 = await userManager.FindByEmailAsync(email4);
            var user5 = await userManager.FindByEmailAsync(email5);
            var user6 = await userManager.FindByEmailAsync(email6);

            await userManager.AddToRolesAsync(user1, new string[] { RoleConstants.Manager, RoleConstants.Administrator});
            await userManager.AddToRoleAsync(user3, RoleConstants.Supervisor);
            await userManager.AddToRoleAsync(user5, RoleConstants.Supervisor);
            await userManager.AddToRoleAsync(user4, RoleConstants.Administrator);
            await userManager.AddToRoleAsync(user2, RoleConstants.Administrator);
            await userManager.AddToRoleAsync(user6, RoleConstants.Guest);

            return RedirectToAction("Index", "Home");
        }
    }
}
