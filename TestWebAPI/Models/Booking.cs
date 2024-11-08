using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Models.Base;


namespace Models;

public class Booking : Entity {

    public Event Event { get; set; } = new Event();

    public int NumberOfTickets { get; set; }

    public DateTime BookingDate { get; set; }

}