using BarberApi.Data;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;

namespace BarberApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services)
        {
            Env.Load();
            var connectionString = $"Server={Environment.GetEnvironmentVariable("DB_HOST")};"+
                                   $"Port={Environment.GetEnvironmentVariable("DB_PORT")};"+
                                   $"Database={Environment.GetEnvironmentVariable("DB_NAME")};"+
                                   $"User={Environment.GetEnvironmentVariable("DB_USER")};"+
                                   $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD")};";
            services.AddDbContext<BarberShopDbContext>(options => options.UseMySQL(connectionString));
            return services;
        }
    }
    
    
}