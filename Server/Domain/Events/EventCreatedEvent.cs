namespace Server.Domain.Events;

public record EventCreatedEvent(
    Guid EventId, 
    string Title, 
    DateTime StartDate, 
    Guid VenueId, 
    int MaxCapacity
    );