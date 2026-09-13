using Server.Domain.Models.Enums;

namespace Server.Domain.Models;

public class Event
{
    public Guid Id {get; private set; }
    public string Title {get; private set; }
    public DateTime StartDate {get; private set; }
    public Guid VenueId {get; private set; }
    public int MaxCapacity {get; private set; }
        
    private readonly List<Registration> _registrations = [];
    public IReadOnlyCollection<Registration> Registrations => _registrations.AsReadOnly();
    
    public Event(Guid id, string title, DateTime startDate, Guid venueId, int maxCapacity)
    {
        Id = id;
        Title = title;
        StartDate = startDate;
        VenueId = venueId;
        MaxCapacity = maxCapacity;
    }
    
    
    public Registration RegisterParticipant(Guid participantId)
    {
        var activeRegistrations = _registrations.Count(r => r.Status == RegistrationStatus.Confirmed);
        if (activeRegistrations >= MaxCapacity)
            throw new InvalidOperationException("Достигнута максимальная вместимость мероприятия.");

        if (_registrations.Any(r => r.ParticipantId == participantId && r.Status == RegistrationStatus.Confirmed))
            throw new InvalidOperationException("Участник уже зарегистрирован на это мероприятие.");

        var registration = new Registration(Guid.NewGuid(), Id, participantId);
        _registrations.Add(registration);
        return registration;
    }
}