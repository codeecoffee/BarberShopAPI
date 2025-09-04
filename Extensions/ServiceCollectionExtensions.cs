using System.Text;
using BarberApi.Data;
using BarberApi.Services;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<ITokenService, TokenService>();
            return services;
        }
        public static IServiceCollection AddSwaggerWithJwt(this IServiceCollection services)
        {
            services.AddOpenApi();
            services.AddEndpointsApiExplorer();
            return services;
        }
    }
    
    
}