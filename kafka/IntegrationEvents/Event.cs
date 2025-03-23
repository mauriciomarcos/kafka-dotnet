namespace kafka.IntegrationEvents;

public abstract class Event : IEvent
{
    public DateTime Timestamp { get; private set; }

    protected Event()
    {
        Timestamp = DateTime.Now;
    }
}