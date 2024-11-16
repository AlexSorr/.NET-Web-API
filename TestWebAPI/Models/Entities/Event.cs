using System.ComponentModel.DataAnnotations.Schema;
using API.Models.Base;
using BookingStatus = API.Models.Enums.BookingStatus;

namespace API.Models;

/// <summary>
/// Представляет событие в системе, включая его название, локацию, дату и билеты.
/// </summary>
public class Event : Entity, IParsableEntity<Event> {

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="Event"/>.
    /// Конструктор по умолчанию. 
    /// </summary>
    public Event() { }

    /// <summary>
    /// Название мероприятия.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Локация мероприятия.
    /// </summary>
    public Location Location { get; set; } = new Location();

    /// <summary>
    /// Дата проведения мероприятия.
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Список всех билетов для мероприятия.
    /// </summary>
    public List<Ticket> Tickets { get; set; } = new List<Ticket>();

    /// <summary>
    /// Доступные для бронирования билеты (статус <c>Free</c>).
    /// </summary>
    [NotMapped]
    public IEnumerable<Ticket> AvailableTickets => Tickets.Where(t => t.BookingStatus == BookingStatus.Free);

    /// <summary>
    /// Забронированные билеты (статус <c>Booked</c>).
    /// </summary>
    [NotMapped]
    public IEnumerable<Ticket> BookedTickets => Tickets.Where(t => t.BookingStatus == BookingStatus.Booked);

    /// <summary>
    /// Указывает, проданы ли все билеты на мероприятие.
    /// </summary>
    public bool IsSoldOut => Tickets.Any() && AvailableTickets.Count() <= 0;

    /// <inheritdoc/>
    public static Event Parse(object? @object) {
        return @object as Event;
    }
    
    /// <inheritdoc/>
    public static bool TryParse(object? @object, out Event? result) {
        result = @object as Event;
        return result != null;
    }
}
