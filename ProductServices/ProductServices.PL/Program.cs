
using ProductServices.DAL.Context;
using Microsoft.EntityFrameworkCore;
using ProductServices.DAL.Repositories.Interfaces;
using ProductServices.DAL.Repositories.Classes;
using ProductServices.BLL.Services.Classes;
using ProductServices.BLL.Services.Interfaces;
using ProductServices.BLL.Services;
using ProductServices.BLL.Services.Import;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<ProductServicesDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositories
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();

// Services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
// Import Services
builder.Services.AddScoped<IProductImportService, ProductImportService>();
builder.Services.AddScoped<IProductCsvParser, ProductCsvParser>();
builder.Services.AddScoped<IProductImportValidator, ProductImportValidator>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseStaticFiles();

app.MapControllers();

app.Run();
