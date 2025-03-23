using Confluent.Kafka;
using kafka.IntegrationEvents;
using kafka.Interfaces;

namespace kafka;

public class MessageBus(string bootstrapServer) : IMessageBus
{
    public async Task ProducerAsync<T>(string topic, T Message, CancellationToken cancellationToken = default) where T : IEvent
    {
        ProducerConfig producerConfiguration = new() { BootstrapServers = bootstrapServer };

        var producer = new ProducerBuilder<string, T>(producerConfiguration).Build();

        _ = await producer.ProduceAsync(topic, new Message<string, T>
        {
            Key = Guid.NewGuid().ToString(),
            Value = Message
        }, cancellationToken);

        await Task.CompletedTask;
    }

    public Task ConsumerAsync<T>(string topic, Func<T, Task> onMessage, CancellationToken cancellationToken) where T : IEvent
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}