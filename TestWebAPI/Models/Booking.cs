namespace Models;

public class Booking : IEntity {
     
    public long Id { get; }

    
    public Event Event { get; set; } =  new Event();
    public long EventId { get => Event?.Id ?? 0; }

    public int NumberOfTickets { get; set; }
    public DateTime BookingDate { get; set; }

}