namespace Server.Domain.Models;

public class Participant
{
    public Guid Id {get; private set; }
    public string FullName {get; private set; }
    public string Email {get; private set; }

    public Participant(Guid id, string fullName, string email)
    {
        Id = id;
        FullName = fullName;
        Email = email;
    }
}