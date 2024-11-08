
using API.Models.Base;

namespace API.Models;

public class Ticket : Entity {

    public Ticket () { }
    
    public string Number { get; set; } = string.Empty;

    public Event Event { get; set; } = new Event();

    public DateTime EventDate => this.Event?.Date ?? DateTime.MinValue; 

    /// <summary>
    /// Бронирование
    /// </summary>
    private bool _is_booked = false;
    public DateTime BookingDate { get; private set; }
    public bool IsBooked { 
        get => _is_booked; 
        set {
            DateTime bookingDate = value ? DateTime.Now : DateTime.MinValue;
            _is_booked = value; BookingDate = bookingDate;
        }
    }

}