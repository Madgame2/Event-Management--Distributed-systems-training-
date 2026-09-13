using Server.Domain.Models.Enums;

namespace Server.Domain.Models;

public class Registration
{
    public Guid Id {get; private set; }
    public Guid EventId { get; private set; }
    public Guid ParticipantId {get; private set; }
    public RegistrationStatus Status {get; private set; } = RegistrationStatus.Confirmed;
    public DateTime RegisteredAt {get; private set; } = DateTime.UtcNow;

    public void Cancel() => Status = RegistrationStatus.Cancelled;
    
    public Registration(Guid id, Guid eventId, Guid participantId)
    {
        Id = id;
        EventId = eventId;
        ParticipantId = participantId;
    }
}