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
    public virtual Event Event { get; set; } = new Event();

    /// <summary>
    /// Дата мероприятия
    /// </summary>
    public DateTime EventDate => this.Event?.Date ?? DateTime.MinValue; 

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
                    this.BookingDate = DateTime.MinValue;
                    this.SellingDate = DateTime.MinValue;
                    break;
                case BookingStatus.Booked:
                    this.BookingDate = DateTime.Now;
                    this.SellingDate = DateTime.MinValue;
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
    public DateTime BookingDate { get; private set; }

    /// <summary>
    /// Дата продажи
    /// </summary>
    public DateTime SellingDate { get; private set; }
    
    /// <summary>
    /// Зарезервирован
    /// </summary>
    public bool IsBooked => this.BookingStatus == BookingStatus.Booked;

    /// <summary>
    /// Продан
    /// </summary>
    public bool IsSelled => this.BookingStatus == BookingStatus.Selled;

}