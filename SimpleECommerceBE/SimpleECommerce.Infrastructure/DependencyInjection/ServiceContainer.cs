using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedLibPackage.DependencyInjection;
using SimpleECommerce.Application.Interfaces;
using SimpleECommerce.Application.IService;
using SimpleECommerce.Infrastructure.Data;
using SimpleECommerce.Infrastructure.Repositories;
using SimpleECommerce.Infrastructure.Service;
using System.Text.Json.Serialization;

namespace SimpleECommerce.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config)
        {

            // Access the environment variables
            string dataBaseConnection = config.GetConnectionString("DefaultConnection")!;

            // Adding Json options for the controll of infinite loop in the many to many relationships
            services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            // Add database context for Npgsql
            services.AddDbContext<SimpleECommerceDbContext>(options =>
            options.UseSqlServer(dataBaseConnection));


            //Add database connectivity
            //Add authentication scheme
            SharedServiceContainer.AddSharedService<SimpleECommerceDbContext>(services, config, config["MySerilog:FileName"]!);

            // create Dependency Injection (DI)
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<ICartService, CartService>();

            return services;
        }

        // Method to add middleware piplines
        public static IApplicationBuilder UseInfrastructurePolicy(this IApplicationBuilder app)
        {
            // Register middleware such as:
            // ExceptionHandlingMiddleware: which handles external errors
            // ListToOnlyAPIGatway: which blocks all outsider calls to the api
            SharedServiceContainer.UseSharedPolicies(app);

            return app;
        }
    }
}
