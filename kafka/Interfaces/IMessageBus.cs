using kafka.IntegrationEvents;

namespace kafka.Interfaces;

public interface IMessageBus : IDisposable
{
    Task ProducerAsync<T>(string topic, T message, CancellationToken cancellationToken = default) where T : IEvent;
    Task ConsumerAsync<T>(string topic, Func<T, Task> onMessage, CancellationToken cancellationToken) where T : IEvent;
}