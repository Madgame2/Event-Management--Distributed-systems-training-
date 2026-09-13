namespace ConsoleClinet.Services;

public interface IEventApiClient
{
    Task<string> CreateEventAsync(CreateEventCommand command);
    Task<string> RegisterParticipantAsync(RegisterParticipantCommand command);
}