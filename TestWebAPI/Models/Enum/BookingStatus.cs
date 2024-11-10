namespace API.Models;

/// <summary>
/// Статус бронирования билета.
/// </summary>
public enum BookingStatus {
    /// <summary>
    /// Билет доступен для бронирования или продажи.
    /// </summary>
    Free,

    /// <summary>
    /// Билет забронирован, но не выкуплен.
    /// </summary>
    Booked,

    /// <summary>
    /// Билет выкуплен, статус завершен.
    /// </summary>
    Selled
}
