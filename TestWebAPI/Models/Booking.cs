using API.Models.Base;

namespace API.Models;

public class Booking : Entity {

    public Event Event { get; set; } = new Event();

    public int NumberOfTickets { get; set; }

    public DateTime BookingDate { get; set; }

}