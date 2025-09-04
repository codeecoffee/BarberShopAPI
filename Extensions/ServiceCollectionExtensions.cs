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

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
        {
            var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");
            var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
            var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");

            if (string.IsNullOrEmpty(jwtKey))
                throw new InvalidOperationException("JWT_KEY environment variable is required");

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtIssuer,
                        ValidAudience = jwtAudience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                        ClockSkew = TimeSpan.Zero
                    };
                });
            services.AddAuthorization();
            return services;
        }
    }
    
    
}