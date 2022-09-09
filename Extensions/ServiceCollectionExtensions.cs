using AutoMapper;
using INVENTORY.SERVER.Data;
using INVENTORY.SERVER.Data.Interfaces;
using INVENTORY.SERVER.Services;
using INVENTORY.SERVER.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace INVENTORY.SERVER.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IProductRepository, ProductRepository>();
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<IProductService, ProductService>();
        }

        public static void AddDBConfiguration(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<InventoryDBContext>(
                option => option.UseSqlServer(connectionString)
                                .EnableSensitiveDataLogging()
                );
        }

        public static void AddAutoMapperConfiguration(this IServiceCollection services)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ProductProfile());
            });

            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);
        }

        public static void AddLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }

        public static void AddCorsDtosConfiguration(this IServiceCollection services, string MyAllowSpecificOrigins)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  builder =>
                                  {
                                      builder.WithOrigins
                                      ("http://localhost:4200", "http://localhost:3000", "https://localhost:7170");
                                      builder.AllowAnyHeader();
                                      builder.AllowAnyMethod();
                                      builder.AllowCredentials();
                                  });
            });
        }
    }
}


