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
    public Location Location { get; set; } = new Location();

    /// <summary>
    /// Дата мероприятия
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Список билетов
    /// </summary>
    public List<Ticket> Tickets { get; set; } = new List<Ticket>();
    
    /// <summary>
    ///  Доступные билеты
    /// </summary>
    [NotMapped]
    public IEnumerable<Ticket> AvailableTickets => Tickets.Where(t => t.BookingStatus == BookingStatus.Free);

    /// <summary>
    /// Забронированные билеты
    /// </summary>
    [NotMapped]
    public IEnumerable<Ticket> BookedTickets => Tickets.Where(t => t.BookingStatus == BookingStatus.Booked);

    /// <summary>
    /// Sold out
    /// </summary>
    public bool IsSoldOut => Tickets.Any() && AvailableTickets.Count() <= 0;


}