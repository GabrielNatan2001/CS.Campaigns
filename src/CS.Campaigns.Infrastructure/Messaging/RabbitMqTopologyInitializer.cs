using CS.Campaigns.Application.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CS.Campaigns.Infrastructure.Messaging;

public class RabbitMqTopologyInitializer : IHostedService
{
    private readonly IServiceProvider _sp;
    private readonly ILogger<RabbitMqTopologyInitializer> _logger;

    public RabbitMqTopologyInitializer(IServiceProvider sp, ILogger<RabbitMqTopologyInitializer> logger)
    {
        _sp = sp;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var scope = _sp.CreateScope();
            var bus = scope.ServiceProvider.GetRequiredService<IMessageBus>();
            var cfg = scope.ServiceProvider.GetRequiredService<IOptions<DoacaoRecebidaPublisherConfig>>().Value;
            bus.EnsureTopology(cfg.Exchange, cfg.RoutingKey);
            _logger.LogInformation("RabbitMQ topology ensured for {Exchange}/{RoutingKey}", cfg.Exchange, cfg.RoutingKey);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Falha ao garantir topologia RabbitMQ no startup");
        }
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}

public static class RabbitMqTopologyExtensions
{
    public static IServiceCollection AddRabbitMqTopologyInitializer(this IServiceCollection services)
    {
        services.AddHostedService<RabbitMqTopologyInitializer>();
        return services;
    }
}
