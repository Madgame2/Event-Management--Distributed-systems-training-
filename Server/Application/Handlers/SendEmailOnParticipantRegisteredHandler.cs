using Server.Application.Services.EventPublisher;
using Server.Domain.Events;

namespace Server.Application.Handlers;

public class SendEmailOnParticipantRegisteredHandler : IEventHandler<ParticipantRegisteredEvent>
{
    public Task HandleAsync(ParticipantRegisteredEvent @event) 
    {
        Console.WriteLine($"[Email Service] Отправляем письмо участнику {@event.ParticipantId} о регистрации {@event.RegistrationId}");
        
        return Task.CompletedTask;
    }
}