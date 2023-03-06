using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebShopDemo.Core.Contracts;
using WebShopDemo.Core.Services;
using WebShopDemo.Core.Data;
using WebShopDemo.Core.Data.Common;
using WebShopDemo.Core.Data.Models.Account;
using System.Configuration;
using Microsoft.EntityFrameworkCore.Internal;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("SqlConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 6;
    
    options.User.RequireUniqueEmail = true;

    options.Lockout.MaxFailedAccessAttempts = 5;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.ConfigureApplicationCookie(options => 
{
    options.LoginPath = "/Account/Login";
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanDeleteProduct", policy => policy.RequireAssertion(context => context.User.IsInRole(RoleConstants.Manager) && context.User.IsInRole(RoleConstants.Administrator)));
});

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IRepository, Repository>();

//builder.Services.AddDistributedMemoryCache();
//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.FromMinutes(10);
//    options.Cookie.HttpOnly = true;
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.Use((context, next) =>
{
    context.Request.Scheme = "https";
    return next();
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

//app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
