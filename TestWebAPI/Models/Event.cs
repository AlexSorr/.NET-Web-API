namespace Models;

public class Event : IEntity {

    public Event() { }

    public long Id { get; private set;}
    public string Name { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public DateTime Date { get; set; }
    public int AvailableTickets { get; set; }

}