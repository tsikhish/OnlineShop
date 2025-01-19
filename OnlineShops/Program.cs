using Microsoft.EntityFrameworkCore;
using OnlineShops.Models;
using OnlineShops.Models.IRepositories;
using OnlineShops.Models.IService;
using OnlineShops.Models.Repositories;
using OnlineShops.Models.Services;
using Microsoft.AspNetCore.Identity;
using OnlineShops;
using OnlineShops.Models.DbModels;
using System.Reflection.Emit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<AdventureWorksLT2019Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
;

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<AdventureWorksLT2019Context>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductCategoryService, ProductCategoryService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ISalesOrderDetailRepository, SalesOrderDetailRepository>();
builder.Services.AddScoped<ISalesOrderHeaderRepository, SalesOrderHeaderRepository>();
builder.Services.AddScoped<ISalesOrderService, SalesOrderService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IUserService, UserService>();

// Register repositories
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
var app = builder.Build();

//await Seed.InitializeAsync(app.Services);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
                name: "default",
               pattern: "{controller=Home}/{action=Index}/{id?}");


app.MapControllerRoute(
     name: "product",
     pattern: "Product/{action=Index}/{id?}",
     defaults: new { controller = "Product" });

app.MapControllerRoute(
    name: "productcategory",
    pattern: "ProductCategory/{action=Index}/{id?}",
    defaults: new { controller = "ProductCategory" });
app.MapControllerRoute(
    name: "salesorder",
    pattern: "SalesOrder/{action=Index}/{id?}",
    defaults: new { controller = "SalesOrder" });
app.MapControllerRoute(
    name: "customer",
    pattern: "Customer/{action=Index}/{id?}",
    defaults: new { controller = "Customer" });
app.MapControllerRoute(
    name: "Address",
    pattern: "Address/{action=Index}/{id?}",
    defaults: new { controller = "Address" });
app.Run();
