using CS.Campaigns.Application.Messaging;
using CS.Campaigns.Domain.Campanhas.Interfaces;
using CS.Campaigns.Domain.Doacoes.Interfaces;
using CS.Campaigns.Infrastructure.Data;
using CS.Campaigns.Infrastructure.Data.Repositories;
using CS.Campaigns.Infrastructure.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CS.Campaigns.Infrastructure;

public static class DependencyInjectionInfrastructure
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<ICampanhaRepository, CampanhaRepository>();
        services.AddScoped<IDoacaoRepository, DoacaoRepository>();
        services.Configure<DoacaoRecebidaPublisherConfig>(configuration.GetSection("Publishers:DoacaoRecebida"));
        services.AddSingleton<IMessageBus, MessageBus>();
        services.AddRabbitMqTopologyInitializer();
        return services;
    }

    public static IServiceCollection AddHealthChecksInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var cs = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("DefaultConnection não configurado.");
        var rabbit = configuration["MessageBusConfigs:Host"]
            ?? throw new InvalidOperationException("MessageBusConfigs:Host não configurado.");

        services.AddHealthChecks()
            .AddNpgSql(cs, name: "postgres")
            .AddRabbitMQ(rabbit, name: "rabbitmq");
        return services;
    }
}
