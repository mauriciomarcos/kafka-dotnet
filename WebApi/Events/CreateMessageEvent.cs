using kafka.IntegrationEvents;

namespace WebApi.Events;

public class CreateMessageEvent(string eventDescription, DateTime createAt, bool status) : Event
{
    public string EventDescription { get; init; } = eventDescription;
    public DateTime CreateAt { get; init; } = createAt;
    public bool Status { get; init; } = status;
}