namespace CS.Campaigns.Application.Messaging;

public interface IMessageBus : IDisposable
{
    void Publish(string exchange, string routingKey, object message);
    void EnsureTopology(string exchange, string routingKey);
}
