using Server.Application.Services.EventPublisher;
using Server.Domain.Events;

namespace Server.Application.Handlers;

public class LogEventCreatedHandler : IEventHandler<EventCreatedEvent>
{
    private readonly ILogger<LogEventCreatedHandler> _logger;

    public LogEventCreatedHandler(ILogger<LogEventCreatedHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(EventCreatedEvent @event)
    {
        _logger.LogInformation("Создано новое мероприятие: {Title} (ID: {EventId})", 
            @event.Title, @event.EventId);
        
        return Task.CompletedTask;
    }
}