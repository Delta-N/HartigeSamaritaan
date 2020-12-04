using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Data.Context;

namespace RoosterPlanner.Service
{
    public static class ServiceContainer
    {
        public static void Register(IServiceCollection services, IConfiguration configuration)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.AddDbContext<RoosterPlannerContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("RoosterPlannerDatabase")));
            services.BuildServiceProvider().GetService<RoosterPlannerContext>()?.Database.Migrate();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
