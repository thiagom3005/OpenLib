using Microsoft.Extensions.DependencyInjection;
using OpenLib.Application.Services;
using OpenLib.Application.Services.Implementations;

namespace OpenLib.Application.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ILivroService, LivroService>();
        services.AddScoped<IEmprestimoService, EmprestimoService>();

        return services;
    }
}
