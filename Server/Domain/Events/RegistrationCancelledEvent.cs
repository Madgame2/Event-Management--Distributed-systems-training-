namespace Server.Domain.Events;

public record RegistrationCancelledEvent(
    Guid RegistrationId, 
    Guid EventId, 
    Guid ParticipantId
    );