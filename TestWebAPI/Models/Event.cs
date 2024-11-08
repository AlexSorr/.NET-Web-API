using API.Models.Base;

namespace API.Models;

public class Event : Entity {

    public Event() { }

    public string Name { get; set; } = string.Empty;

    public virtual Location Location { get; set; } = new Location();

    public DateTime Date { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    
    /// <summary>
    /// Вот это можно не хранить в базе, а рассчитывать количество незабронированных билетов 
    /// </summary>
    public int AvailableTickets { get; set; }

    public bool IsSoldOut => AvailableTickets <= 0;

}