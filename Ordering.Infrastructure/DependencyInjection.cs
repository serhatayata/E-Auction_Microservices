using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ordering.Domain.Repositories.Base;
using Ordering.Infrastructure.Repositories.Base;
using Ordering.Infrastructure.Repositories;

namespace Ordering.Infrastructure
{
    public static class DependencyInjection
    { 
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,IConfiguration configuration)
        {
            //services.AddDbContext<OrderContext>(opt => opt.UseInMemoryDatabase(databaseName: "InMemoryDb"),
            //                                    ServiceLifetime.Singleton,
            //                                   ServiceLifetime.Singleton);

            services.AddDbContext<OrderContext>(
                options => options.UseSqlServer(
                        configuration.GetConnectionString("OrderConnection"),
                        b => b.MigrationsAssembly(typeof(OrderContext).Assembly.FullName)), ServiceLifetime.Singleton);

            services.AddTransient(typeof(IRepository<>), typeof(Repository<>)); //typeof kullanmanın nedeni kendi içinde tip alan interfaceler bu şekilde eklenir. (best practice)
            services.AddTransient<IOrderRepository, OrderRepository>();
            return services;
        }
    }
}
 