using Cloudy.API.Infrastructure.Data;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Minio;

namespace Cloudy.API;

public static class DependencyInjection
{
    public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options => options
            .UseNpgsql(configuration.GetConnectionString("PostgreSqlConnection")));

        services.AddIdentity<IdentityUser<Guid>, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
    }

    public static void AddStorage(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMinio(options => options
            .WithEndpoint(configuration["Minio:Endpoint"])
            .WithCredentials(configuration["Minio:AccessKey"], configuration["Minio:SecretKey"])
            .WithSSL(false));
    }

    public static void AddValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
    }
}