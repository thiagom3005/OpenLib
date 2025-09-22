using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenLib.Application.Abstractions.Repositories;
using OpenLib.Application.Abstractions.UnitOfWork;
using OpenLib.Infrastructure.Persistence;
using OpenLib.Infrastructure.Repositories;

namespace OpenLib.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' não configurada.");

        services.AddDbContext<LibraryDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<ILivroRepository, LivroRepository>();
        services.AddScoped<IEmprestimoRepository, EmprestimoRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

        return services;
    }
}
