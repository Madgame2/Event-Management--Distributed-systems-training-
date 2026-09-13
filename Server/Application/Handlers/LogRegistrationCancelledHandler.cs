using Server.Application.Services.EventPublisher;
using Server.Domain.Events;

namespace Server.Application.Handlers;

public class LogRegistrationCancelledHandler : IEventHandler<RegistrationCancelledEvent>
{
    public Task HandleAsync(RegistrationCancelledEvent @event)
    {
        Console.WriteLine($"[Audit Log] Регистрация {@event.RegistrationId} отменена.");
        
        return Task.CompletedTask;
    }
}