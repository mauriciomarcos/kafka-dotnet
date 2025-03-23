namespace kafka.IntegrationEvents;

public interface IEvent
{
    DateTime Timestamp { get; }
}