using API.Models.Base;

namespace API.Models;

/// <summary>
/// Представляет бронирование для мероприятия, включая информацию о количестве забронированных билетов и дате бронирования.
/// Наследуется от <see cref="Entity"/>.
/// </summary>
public class Booking : Entity, IParsableEntity<Booking> {

    /// <summary>
    /// Мероприятие, для которого было произведено бронирование.
    /// </summary>
    public virtual Event Event { get; set; } = new Event();

    /// <summary>
    /// Количество забронированных билетов.
    /// </summary>
    public int NumberOfTickets { get; set; }

    /// <summary>
    /// Дата и время, когда было произведено бронирование.
    /// </summary>
    public DateTime BookingDate { get; set; }

    /// <inheritdoc/>
    public static Booking Parse(object? @object) {
        return @object as Booking;
    }

    /// <inheritdoc/>
    public static bool TryParse(object? @object, out Booking? result) {
        result = @object as Booking;
        return result != null;
    }
}
