using CS.Campaigns.Application.Campanhas.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CS.Campaigns.Application;

public static class DependencyInjectionApplication
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CriarCampanhaService>();
        services.AddScoped<AtualizarCampanhaService>();
        services.AddScoped<ListarCampanhasService>();
        services.AddScoped<ListarCampanhasAtivasService>();
        services.AddScoped<CriarDoacaoService>();
        return services;
    }
}
