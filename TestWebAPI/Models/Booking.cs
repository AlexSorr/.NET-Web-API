using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Models.Base;


namespace Models;

public class Booking : Entity {

    [Column("Event")]
    public Event Event { get; set; } = new Event();

    [Column("NumberOfTickets")]
    public int NumberOfTickets { get; set; }

    [Column("BookingDate")]
    public DateTime BookingDate { get; set; }

}