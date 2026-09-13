using Server.Application.Services.EventPublisher;
using Server.Domain.Events;

namespace Server.Application.Handlers;

public class GenerateQrCodeOnTicketIssuedHandler : IEventHandler<TicketIssuedEvent>
{
    public Task HandleAsync(TicketIssuedEvent @event)
    {
        Console.WriteLine($"[QR Service] Генерация QR-кода для билета {@event.TicketCode}");
        
        return Task.CompletedTask;
    }
}