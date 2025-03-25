
using kafka.Interfaces;

namespace WebApi.Events.EventHandlers;

public class CreateMessageEventHandler : BackgroundService
{
    private readonly IMessageBus _messageBus;
    private readonly IServiceProvider _serviceProvider;

    public CreateMessageEventHandler(IMessageBus messageBus, IServiceProvider serviceProvider)
    {
        _messageBus = messageBus;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _messageBus.ConsumerAsync<CreateMessageEvent>("created-message", ProcessEvent, stoppingToken);
        await Task.CompletedTask;
    }

    private async Task ProcessEvent(CreateMessageEvent @event)
    {
        using var scope = _serviceProvider.CreateScope();
        Thread.Sleep(1000);

        await Task.CompletedTask;
    }
}