using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using CS.Campaigns.Application.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace CS.Campaigns.Infrastructure.Messaging;

public class MessageBus : IMessageBus
{
    private readonly MessageBusConfigs _busConfigs;
    private readonly ILogger<MessageBus> _logger;
    private readonly ConnectionFactory _factory;
    private bool _isConnected;
    private IConnection? _connection;
    private readonly HashSet<string> _exchanges = new();
    private readonly HashSet<string> _queues = new();

    public MessageBus(IConfiguration configuration, ILogger<MessageBus> logger)
    {
        _busConfigs = new MessageBusConfigs
        {
            Host = configuration["MessageBusConfigs:Host"]
                ?? throw new InvalidOperationException("MessageBusConfigs:Host não configurado."),
            RetryCount = int.Parse(configuration["MessageBusConfigs:RetryCount"] ?? "5")
        };
        _logger = logger;
        _factory = new ConnectionFactory { Uri = new Uri(_busConfigs.Host) };
    }

    private IConnection Connection
    {
        get
        {
            if (!_isConnected || _connection is null || !_connection.IsOpen)
                Connect();
            return _connection!;
        }
    }

    public void Connect()
    {
        var policy = Policy.Handle<SocketException>().Or<BrokerUnreachableException>()
            .WaitAndRetry(_busConfigs.RetryCount, op => TimeSpan.FromSeconds(Math.Pow(2, op)),
                (ex, _) => _logger.LogError(ex, "Couldn't connect to RabbitMQ"));

        policy.Execute(() =>
        {
            _connection = _factory.CreateConnection();
            _isConnected = true;
        });
    }

    public void EnsureTopology(string exchange, string routingKey)
    {
        using var channel = Connection.CreateModel();
        DeclareQueueAndBind(routingKey, exchange, channel);
    }

    private void DeclareExchange(string exchangeName, IModel channel)
    {
        if (_exchanges.Contains(exchangeName)) return;
        channel.ExchangeDeclare(exchangeName + "-exchange", ExchangeType.Direct, durable: true);
        _exchanges.Add(exchangeName);
    }

    private void DeclareQueueAndBind(string routingKey, string exchangeName, IModel channel)
    {
        DeclareExchange(exchangeName, channel);
        if (_queues.Contains(routingKey)) return;
        channel.QueueDeclare(routingKey + "-queue", durable: true, exclusive: false, autoDelete: false);
        channel.QueueBind(routingKey + "-queue", exchangeName + "-exchange", routingKey);
        _queues.Add(routingKey);
    }

    public void Publish(string exchange, string routingKey, object message)
    {
        using var channel = Connection.CreateModel();
        DeclareExchange(exchange, channel);
        var json = JsonSerializer.Serialize(message);
        _logger.LogInformation("[PUBLISH] {Exchange} {RoutingKey} {Message}", exchange, routingKey, json);
        var props = channel.CreateBasicProperties();
        props.Persistent = true;
        channel.BasicPublish(exchange + "-exchange", routingKey, props, Encoding.UTF8.GetBytes(json));
    }

    public void Dispose() => _connection?.Dispose();
}
