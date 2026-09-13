using Server.Application.DTO;
using Server.Domain.Events;
using Server.Domain.Exceptions;
using Server.Domain.Models;
using Server.Domain.Models.Enums;

namespace Server.Application.Services.EventPublisher;

public class EventManagementService
{
    private readonly List<Event> _events = [];
    private readonly List<Ticket> _tickets = [];
    private readonly IEventBus _eventBus;
    
    public EventManagementService(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }
    
    public async Task<Guid> CreateEventAsync(CreateEventCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.Title))
            throw new ValidationException("Название мероприятия не может быть пустым.");

        if (command.MaxCapacity <= 0)
            throw new ValidationException("Вместимость мероприятия должна быть больше 0.");

        if (command.StartDate <= DateTime.UtcNow)
            throw new ValidationException("Дата начала мероприятия должна быть в будущем.");
        
        var @event = new Event(Guid.NewGuid(), command.Title, command.StartDate, command.VenueId, command.MaxCapacity);
        _events.Add(@event);

        await _eventBus.PublishAsync(new EventCreatedEvent(
            @event.Id, @event.Title, @event.StartDate, @event.VenueId, @event.MaxCapacity
        ));

        return @event.Id;
    }
    
    public async Task<Guid> RegisterParticipantAsync(RegisterParticipantCommand command)
    {

        var @event = _events.FirstOrDefault(e => e.Id == command.EventId) 
                     ?? throw new NotFoundException($"Мероприятие с ID {command.EventId} не найдено.");

        try
        {
            var registration = @event.RegisterParticipant(command.ParticipantId);

            await _eventBus.PublishAsync(new ParticipantRegisteredEvent(
                registration.Id, registration.EventId, registration.ParticipantId, registration.RegisteredAt
            ));

            return registration.Id;
        }
        catch (InvalidOperationException ex)
        {
            throw new InvalidOperationDomainException(ex.Message);
        }
    }
    
    public async Task CancelRegistrationAsync(CancelRegistrationCommand command)
    {
        var @event = _events.FirstOrDefault(e => e.Registrations.Any(r => r.Id == command.RegistrationId))
                     ?? throw new NotFoundException($"Регистрация с ID {command.RegistrationId} не найдена.");

        var registration = @event.Registrations.First(r => r.Id == command.RegistrationId);
        
        if (registration.Status == RegistrationStatus.Cancelled)
            throw new InvalidOperationDomainException("Регистрация уже была отменена ранее.");

        registration.Cancel();

        var ticket = _tickets.FirstOrDefault(t => t.RegistrationId == registration.Id);
        ticket?.Cancel();

        await _eventBus.PublishAsync(new RegistrationCancelledEvent(
            registration.Id, registration.EventId, registration.ParticipantId
        ));
    }
    
    public async Task<Guid> IssueTicketAsync(IssueTicketCommand command)
    {
        var registration = _events
                               .SelectMany(e => e.Registrations)
                               .FirstOrDefault(r => r.Id == command.RegistrationId)
                           ?? throw new NotFoundException($"Регистрация с ID {command.RegistrationId} не найдена.");

        if (registration.Status == RegistrationStatus.Cancelled)
            throw new InvalidOperationDomainException("Нельзя выписать билет для отмененной регистрации.");

        if (_tickets.Any(t => t.RegistrationId == command.RegistrationId && t.Status == TicketStatus.Active))
            throw new InvalidOperationDomainException("Билет для данной регистрации уже выписан.");

        var ticketCode = $"TICK-{Guid.NewGuid().ToString()[..8].ToUpper()}";
        var ticket = new Ticket(Guid.NewGuid(), registration.Id, ticketCode);
        _tickets.Add(ticket);

        await _eventBus.PublishAsync(new TicketIssuedEvent(
            ticket.Id, ticket.RegistrationId, ticket.TicketCode
        ));

        return ticket.Id;
    }
}