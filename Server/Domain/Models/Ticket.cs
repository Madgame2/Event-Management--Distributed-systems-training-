using Server.Domain.Models.Enums;

namespace Server.Domain.Models;

public class Ticket
{
    public Guid Id {get; private set; }
    public Guid RegistrationId { get; private set; }
    public string TicketCode {get; private set; }
    public TicketStatus Status {get ; private set; } =  TicketStatus.Active;

    public void Cancel() => Status = TicketStatus.Cancelled;

    public Ticket(Guid id, Guid registrationId, string ticketCode)
    {
        Id = id;
        RegistrationId = registrationId;
        TicketCode = ticketCode;
    }
}