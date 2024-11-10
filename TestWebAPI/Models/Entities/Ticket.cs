using API.Models.Base;

namespace API.Models;

/// <summary>
/// Представляет билет на мероприятие.
/// Наследуется от <see cref="Entity"/>.
/// </summary>
public class Ticket : Entity {

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="Ticket"/>.
    /// Конструктор по умолчанию.
    /// </summary>
    public Ticket () { }

    /// <summary>
    /// Номер билета.
    /// Номер генерируется при создании билета.
    /// </summary>
    public string Number { get; set; } = string.Empty;

    /// <summary>
    /// Мероприятие, к которому привязан билет.
    /// </summary>
    public Event Event { get; set; } = new Event();

    /// <summary>
    /// Дата мероприятия.
    /// Свойство, возвращающее дату события из связанного мероприятия.
    /// </summary>
    public DateTime? EventDate => this.Event?.Date;

    
    private BookingStatus _bookingStatus;
    /// <summary>
    /// Статус бронирования и продажи билета.
    /// В зависимости от статуса обновляются даты бронирования и продажи.
    /// </summary>
    public BookingStatus BookingStatus { 
        get => _bookingStatus; 
        set {
            _bookingStatus = value;
            switch (_bookingStatus) {
                case BookingStatus.Free:
                    this.BookingDate = null;
                    this.SellingDate = null;
                    break;
                case BookingStatus.Booked:
                    this.BookingDate = DateTime.Now;
                    this.SellingDate = null;
                    break;
                case BookingStatus.Selled:
                    this.SellingDate = DateTime.Now;
                    break;
                default: break;
            }
        }
    }

    /// <summary>
    /// Дата бронирования билета.
    /// Применяется, когда билет забронирован.
    /// </summary>
    public DateTime? BookingDate { get; set; }

    /// <summary>
    /// Дата продажи билета.
    /// Применяется, когда билет продан.
    /// </summary>
    public DateTime? SellingDate { get; set; }

    /// <summary>
    /// Проверка, забронирован ли билет.
    /// Возвращает true, если статус бронирования равен <see cref="BookingStatus.Booked"/>.
    /// </summary>
    public bool IsBooked => this.BookingStatus == BookingStatus.Booked;

    /// <summary>
    /// Проверка, продан ли билет.
    /// Возвращает true, если статус бронирования равен <see cref="BookingStatus.Selled"/>.
    /// </summary>
    public bool IsSelled => this.BookingStatus == BookingStatus.Selled;

}
