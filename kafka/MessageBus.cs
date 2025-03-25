using Confluent.Kafka;
using kafka.IntegrationEvents;
using kafka.Interfaces;
using kafka.Serializations;

namespace kafka;

public class MessageBus(string bootstrapServer) : IMessageBus
{
    public async Task ProducerAsync<T>(string topic, T message, CancellationToken cancellationToken = default) where T : IEvent
    {
        ProducerConfig producerConfiguration = new() { BootstrapServers = bootstrapServer };

        //var payload = System.Text.Json.JsonSerializer.Serialize(message);
        //var producer = new ProducerBuilder<string, string>(producerConfiguration).Build();

        var producer = new ProducerBuilder<string, T>(producerConfiguration)
            .SetValueSerializer(new KafkaSerializer<T>())
            .Build();

        _ = await producer.ProduceAsync(topic, new Message<string, T>
        {
            Key = Guid.NewGuid().ToString(),
            Value = message
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

            //using var consumer = new ConsumerBuilder<string, string>(consumerConfiguration).Build();
            using var consumer = new ConsumerBuilder<string, T>(consumerConfiguration)
                .SetValueDeserializer(new KafkaDesserializer<T>())
                .Build();

            consumer.Subscribe(topic);

            while (!cancellationToken.IsCancellationRequested)
            {
                var result = consumer.Consume();
                if (result.IsPartitionEOF)
                    continue;

                //var message = JsonSerializer.Deserialize<T>(result.Message.Value);
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