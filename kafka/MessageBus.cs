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

    public async Task ConsumerAsync<T>(string topic, Func<T, Task> onMessage, CancellationToken cancellationToken) where T : IEvent
    {
        _ = Task.Factory.StartNew(async () =>
        {
            ConsumerConfig consumerConfiguration = new()
            {
                GroupId = "app-group",
                BootstrapServers = bootstrapServer,
                EnableAutoCommit = false,
                EnablePartitionEof = true
            };

            using var consumer = new ConsumerBuilder<string, T>(consumerConfiguration).Build();
            consumer.Subscribe(topic);

            while (!cancellationToken.IsCancellationRequested)
            {
                var result = consumer.Consume();
                if (result.IsPartitionEOF)
                    continue;

                await onMessage(result.Message.Value);

                consumer.Commit();
            }
        }, cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);

        await Task.CompletedTask;
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}