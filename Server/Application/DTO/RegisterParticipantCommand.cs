namespace Server.Application.DTO;

public record RegisterParticipantCommand(Guid EventId, Guid ParticipantId);