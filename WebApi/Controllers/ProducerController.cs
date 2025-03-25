using kafka.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApi.Commands;
using WebApi.Events;

namespace WebApi.Controllers;

[ApiController]
[Route("/api/producer")]
public class ProducerController(IMessageBus messageBus) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateMessage([FromBody] CreateMessageCommand createMessageCommand, CancellationToken cancellationToken)
    {
        var newEvent = new CreateMessageEvent(createMessageCommand.EventDescription, createMessageCommand.CreateAt, createMessageCommand.Status);

        await messageBus.ProducerAsync("created-message", newEvent, cancellationToken);
        return Ok();
    }
}