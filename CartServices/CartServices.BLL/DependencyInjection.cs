using CartServices.BLL.Interfaces;
using CartServices.BLL.Services;
using CartServices.DAL.Data;
using CartServices.DAL.Interfaces;
using CartServices.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CartServices.BLL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCartServices(this IServiceCollection services, IConfiguration configuration)
        {
            var conn = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(conn))
                throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");

            services.AddDbContext<CartDbContext>(opt => opt.UseSqlServer(conn));

            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<ICartService, CartService>();

            // ProductClient registration expects ProductService:BaseUrl in configuration
            var baseUrl = configuration.GetSection("ProductService")?[
                "BaseUrl"] ?? string.Empty;

            services.AddHttpClient<IProductClient, ProductClient>(client =>
            {
                if (!string.IsNullOrWhiteSpace(baseUrl))
                    client.BaseAddress = new Uri(baseUrl);
            });

            return services;
        }
    }
}
