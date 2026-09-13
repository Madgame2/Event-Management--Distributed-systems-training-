namespace Server.Domain.Models;

public class Venue
{
    public Guid Id {get ;private set; }
    public string Name {get ; private set; }
    public string Address {get ; private set; }
    public int Capacity {get ; private set; }

    public Venue(Guid id, string name, string address, int capacity)
    {
        Id = id;
        Name = name;
        Address = address;
        Capacity = capacity;
    }
}