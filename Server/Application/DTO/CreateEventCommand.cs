namespace Server.Application.DTO;

public record CreateEventCommand(string Title, DateTime StartDate, Guid VenueId, int MaxCapacity);