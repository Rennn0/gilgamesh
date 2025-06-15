using Core.DDD.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Core._3D.Ef
{
    public static class Exntesions
    {
        public static IServiceCollection WithEventStoreMsSql(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<EventStoreContext>(options =>
            {
                options.UseSqlServer(connectionString);
                options.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
            });

            services.AddSingleton<IEventStore, EventStore>();
            return services;
        }
    }
}
