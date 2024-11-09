using API.Models.Base;

namespace API.Models;

public class Ticket : Entity {

    public Ticket () { }
    
    /// <summary>
    /// Номер билета
    /// Надо как-то генерить при создании
    /// </summary>
    public string Number { get; set; } = string.Empty;

    /// <summary>
    /// Мероприятие
    /// </summary>
    public Event Event { get; set; } = new Event();

    /// <summary>
    /// Дата мероприятия
    /// </summary>
    public DateTime? EventDate => this.Event?.Date; 

    /// <summary>
    /// Бронирование и продажа
    /// </summary>
    private BookingStatus _bookingStatus;
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
    /// Дата бронирования
    /// </summary>
    public DateTime? BookingDate { get; set; }

    /// <summary>
    /// Дата продажи
    /// </summary>
    public DateTime? SellingDate { get; set; }
    
    /// <summary>
    /// Зарезервирован
    /// </summary>
    public bool IsBooked => this.BookingStatus == BookingStatus.Booked;

    /// <summary>
    /// Продан
    /// </summary>
    public bool IsSelled => this.BookingStatus == BookingStatus.Selled;

}