using System.ComponentModel.DataAnnotations.Schema;
using API.Models.Base;

namespace API.Models;

public class Event : Entity {

    public Event() { }

    /// <summary>
    /// Название
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Локация
    /// </summary>
    public virtual Location Location { get; set; } = new Location();

    /// <summary>
    /// Дата мероприятия
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Список билетов
    /// </summary>
    public virtual ICollection<Ticket> Tickets { get; } = new List<Ticket>();
    
    /// <summary>
    ///  Доступные билеты
    /// </summary>
    [NotMapped]
    public virtual IEnumerable<Ticket> AvailableTickets => Tickets.Where(t => t.BookingStatus == BookingStatus.Free);

    /// <summary>
    /// Забронированные билеты
    /// </summary>
    [NotMapped]
    public virtual IEnumerable<Ticket> BookedTickets => Tickets.Where(t => t.BookingStatus == BookingStatus.Booked);

    /// <summary>
    /// Sold out
    /// </summary>
    public bool IsSoldOut => AvailableTickets.Count() <= 0;


}