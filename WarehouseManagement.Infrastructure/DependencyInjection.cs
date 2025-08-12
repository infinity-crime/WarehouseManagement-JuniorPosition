using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagement.Domain.Interfaces;
using WarehouseManagement.Infrastructure.Data;
using WarehouseManagement.Infrastructure.Data.Repositories;
using WarehouseManagement.Infrastructure.Data.Repositories.Common;

namespace WarehouseManagement.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(connectionString)
                .EnableSensitiveDataLogging()
                .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IResourceRepository, ResourceRepository>();
            services.AddScoped<IUnitOfMeasureRepository, UnitOfMeasureRepository>();
            services.AddScoped<IReceiptResourceRepository, ReceiptResourceRepository>();
            services.AddScoped<IReceiptDocumentRepository, ReceiptDocumentRepository>();

            return services;
        }
    }
}
