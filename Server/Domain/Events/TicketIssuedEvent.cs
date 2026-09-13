namespace Server.Domain.Events;

public record TicketIssuedEvent(
    Guid TicketId, 
    Guid RegistrationId, 
    string TicketCode
    );