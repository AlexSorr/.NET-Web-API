using System.ComponentModel.DataAnnotations.Schema;

using Models.Base;


namespace Models;

public class Event : Entity {

    public Event() { }

    [Column("Name")]
    public string Name { get; set; } = string.Empty;

    [Column("Location")]
    public string Location { get; set; } = string.Empty;

    [Column("Date")]
    public DateTime Date { get; set; }
    
    [Column("AvailableTickets")]
    public int AvailableTickets { get; set; }

}