namespace WebApi.Commands;

public record CreateMessageCommand(string EventDescription, DateTime CreateAt, bool Status);