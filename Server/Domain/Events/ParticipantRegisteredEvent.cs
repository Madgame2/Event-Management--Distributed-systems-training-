namespace Server.Domain.Events;

public record ParticipantRegisteredEvent(
    Guid RegistrationId, 
    Guid EventId, 
    Guid ParticipantId, 
    DateTime RegisteredAt
    );